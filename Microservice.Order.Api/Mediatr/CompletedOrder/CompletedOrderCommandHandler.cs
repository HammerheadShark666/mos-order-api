using AutoMapper;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.Mediatr.CompletedOrder.Model;
using System.Text.Json;

namespace Microservice.Order.Api.MediatR.CompletedOrder;

public class CompletedOrderCommandHandler(IOrderRepository orderRepository,
                                          IAzureServiceBusHelper azureServiceBusHelper,
                                          ICustomerAddressService customerAddressService,
                                          ICustomerHttpAccessor customerHttpAccessor,
                                          ILogger<CompletedOrderCommandHandler> logger,
                                          IMapper mapper) : IRequestHandler<CompletedOrderRequest, CompletedOrderResponse>
{
    private ICustomerAddressService _customerAddressService { get; set; } = customerAddressService;
    private IOrderRepository _orderRepository { get; set; } = orderRepository;
    private IAzureServiceBusHelper _azureServiceBusHelper { get; set; } = azureServiceBusHelper;
    private ICustomerHttpAccessor _customerHttpAccessor { get; set; } = customerHttpAccessor;
    private IMapper _mapper { get; set; } = mapper;
    private ILogger<CompletedOrderCommandHandler> _logger { get; set; } = logger;

    public async Task<CompletedOrderResponse> Handle(CompletedOrderRequest completedOrderRequest, CancellationToken cancellationToken)
    {
        var order = await UpdateOrderToCompleted(completedOrderRequest.OrderId);
        await SendOrderHistoryToServiceBusQueueAsync(order);

        return new CompletedOrderResponse("Order completed.");
    }

    private async Task<Domain.Order> UpdateOrderToCompleted(Guid orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, GetCustomerId());
        if (order == null)
        {
            _logger.LogError($"Order not found for order - {orderId}");
            throw new NotFoundException("Order not found for order.");
        }

        order.OrderStatusId = Enums.OrderStatus.Completed;

        await _orderRepository.UpdateAsync(order);

        return order;
    }

    private Guid GetCustomerId()
    {
        var customerId = _customerHttpAccessor.CustomerId;
        if (Guid.Empty.Equals(customerId))
        {
            _logger.LogError($"Customer not found - {customerId}");
            throw new NotFoundException("Customer not found.");
        }

        return customerId;
    }

    private string GetSerializedOrder(OrderHistory orderHistory)
    {
        return JsonSerializer.Serialize(orderHistory);
    }

    private async Task SendOrderHistoryToServiceBusQueueAsync(Domain.Order order)
    {
        var orderHistory = _mapper.Map<OrderHistory>(order);

        await GetOrderAddress(orderHistory, order);
        await _azureServiceBusHelper.SendMessage(EnvironmentVariables.AzureServiceBusQueueOrderCompleted, GetSerializedOrder(orderHistory));
    }

    private async Task GetOrderAddress(OrderHistory orderHistory, Domain.Order order)
    {
        var customerAddress = await _customerAddressService.GetCustomerAddressAsync(order.CustomerAddressId);
        if (customerAddress == null)
        {
            _logger.LogError($"Customer address not found for order id - {order.Id}");
            throw new NotFoundException("Customer address not found for order id.");
        }

        orderHistory.Address = new OrderHistoryAddress()
        {
            AddressLine1 = customerAddress.AddressLine1,
            AddressLine2 = customerAddress.AddressLine2,
            AddressLine3 = customerAddress.AddressLine3,
            TownCity = customerAddress.TownCity,
            County = customerAddress.County,
            Country = customerAddress.Country,
            Postcode = customerAddress.Postcode
        };
    }
}