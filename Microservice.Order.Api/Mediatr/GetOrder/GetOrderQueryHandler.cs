using AutoMapper;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;

namespace Microservice.Order.Api.MediatR.GetOrder;

public class GetOrderQueryHandler(IOrderRepository orderRepository,
                                  IMapper mapper,
                                  ILogger<GetOrderQueryHandler> logger,
                                  ICustomerAddressService customerAddressService) : IRequestHandler<GetOrderRequest, GetOrderResponse>
{
    private ICustomerAddressService _customerAddressService { get; set; } = customerAddressService;
    private IOrderRepository _orderRepository { get; set; } = orderRepository;
    private IMapper _mapper { get; set; } = mapper;
    private ILogger<GetOrderQueryHandler> _logger { get; set; } = logger;

    public async Task<GetOrderResponse> Handle(GetOrderRequest getOrderRequest, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.OrderSummaryReadOnlyAsync(getOrderRequest.Id);
        if (order == null)
        {
            _logger.LogError($"Order not found for order - {getOrderRequest.Id}");
            throw new NotFoundException($"Order not found for order.");
        }  

        var customerAddress = await _customerAddressService.GetCustomerAddressAsync(order.CustomerAddressId);
        if (customerAddress == null)
        {
            _logger.LogError($"Customer address not found for order - {getOrderRequest.Id}");
            throw new NotFoundException($"Customer address not found for order.");
        }

        return GetOrderResponse(order, customerAddress);
    }

    private GetOrderResponse GetOrderResponse(Domain.Order order, Protos.CustomerAddressResponse customerAddress)
    {
        var orderItemsResponse = _mapper.Map<List<GetOrderOrderItemResponse>>(order.OrderItems);
        var addressResponse = _mapper.Map<GetOrderAddressResponse>(customerAddress);

        return new GetOrderResponse(order.Id, OrderHelper.PaddedOrderNumber(order.OrderNumber),
                                    order.AddressSurname, order.AddressForename, orderItemsResponse,
                                    order.Total, order.OrderStatus.Status,
                                    DateOnly.FromDateTime(order.Created).ToString(Constants.DateFormat_ddMMyyyy),
                                    addressResponse);
    }
}