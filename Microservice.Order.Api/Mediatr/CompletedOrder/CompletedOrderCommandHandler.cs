using AutoMapper;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.Mediatr.CompletedOrder.Model;
using System.Text.Json;
using System.Transactions;

namespace Microservice.Order.Api.MediatR.CompletedOrder;

public class CompletedOrderCommandHandler(IOrderRepository orderRepository,
                                          IAzureServiceBusHelper azureServiceBusHelper,
                                          ICustomerAddressService customerAddressService,
                                          ICustomerHttpAccessor customerHttpAccessor,
                                          ILogger<CompletedOrderCommandHandler> logger,
                                          IMapper mapper) : IRequestHandler<CompletedOrderRequest, CompletedOrderResponse>
{
    public async Task<CompletedOrderResponse> Handle(CompletedOrderRequest completedOrderRequest, CancellationToken cancellationToken)
    {
        using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var order = await UpdateOrderToCompleted(completedOrderRequest.OrderId);

            using (var tx = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                await SendOrderHistoryToServiceBusQueueAsync(order);
                tx.Complete();
            }
            ts.Complete();
        }

        return new CompletedOrderResponse("Order completed.");
    }

    private async Task<Domain.Order> UpdateOrderToCompleted(Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId, GetCustomerId());
        if (order == null)
        {
            logger.LogError("{message}", "Order not found for order - {orderId}");
            throw new NotFoundException("Order not found for order.");
        }

        order.OrderStatusId = Enums.OrderStatus.Completed;

        await orderRepository.UpdateAsync(order);

        return order;
    }

    private Guid GetCustomerId()
    {
        var customerId = customerHttpAccessor.CustomerId;
        if (Guid.Empty.Equals(customerId))
        {
            logger.LogError("{message}", "Customer not found - {customerId}");
            throw new NotFoundException("Customer not found.");
        }

        return customerId;
    }

    private static string GetSerializedOrder(OrderHistory orderHistory)
    {
        return JsonSerializer.Serialize(orderHistory);
    }

    private async Task SendOrderHistoryToServiceBusQueueAsync(Domain.Order order)
    {
        var orderHistory = mapper.Map<OrderHistory>(order);

        await GetOrderAddress(orderHistory, order);
        await azureServiceBusHelper.SendMessage(EnvironmentVariables.AzureServiceBusQueueOrderCompleted, GetSerializedOrder(orderHistory));
    }

    private async Task GetOrderAddress(OrderHistory orderHistory, Domain.Order order)
    {
        var customerAddress = await customerAddressService.GetCustomerAddressAsync(order.CustomerAddressId);
        if (customerAddress == null)
        {
            logger.LogError("{message}", "Customer address not found for order id - {order.Id}");
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