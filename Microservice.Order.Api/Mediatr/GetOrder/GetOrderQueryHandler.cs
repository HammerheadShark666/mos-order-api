using AutoMapper;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interfaces;

namespace Microservice.Order.Api.MediatR.GetOrder;

public class GetOrderQueryHandler(IOrderRepository orderRepository,
                                  IMapper mapper,
                                  IOrderHelper orderHelper,
                                  ILogger<GetOrderQueryHandler> logger,
                                  ICustomerAddressService customerAddressService) : IRequestHandler<GetOrderRequest, GetOrderResponse>
{
    public async Task<GetOrderResponse> Handle(GetOrderRequest getOrderRequest, CancellationToken cancellationToken)
    {
        var order = await orderRepository.OrderSummaryReadOnlyAsync(getOrderRequest.Id);
        if (order == null)
        {
            logger.LogError("Order not found for order - {getOrderRequest.Id}", getOrderRequest.Id);
            throw new NotFoundException("Order not found for order.");
        }

        var customerAddress = await customerAddressService.GetCustomerAddressAsync(order.CustomerAddressId);
        if (customerAddress == null)
        {
            logger.LogError("Customer address not found for order - {getOrderRequest.Id}", getOrderRequest.Id);
            throw new NotFoundException("Customer address not found for order.");
        }

        return GetOrderResponse(order, customerAddress);
    }

    private GetOrderResponse GetOrderResponse(Domain.Order order, Protos.CustomerAddressResponse customerAddress)
    {
        var orderItemsResponse = mapper.Map<List<GetOrderOrderItemResponse>>(order.OrderItems);
        var addressResponse = mapper.Map<GetOrderAddressResponse>(customerAddress);

        return new GetOrderResponse(order.Id, orderHelper.PaddedOrderNumber(order.OrderNumber),
                                    order.AddressSurname, order.AddressForename, orderItemsResponse,
                                    order.Total, order.OrderStatus.Status,
                                    DateOnly.FromDateTime(order.Created).ToString(Constants.DateFormat_ddMMyyyy),
                                    addressResponse);
    }
}