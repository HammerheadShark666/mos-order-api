﻿using AutoMapper;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Domain;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Helpers.Interfaces;
using Microservice.Order.Api.Protos;

namespace Microservice.Order.Api.MediatR.AddOrder;

public class AddOrderCommandHandler(IOrderRepository orderRepository,
                                    IBookService bookService,
                                    IOrderHelper orderHelper,
                                    ICustomerAddressService customerAddressService,
                                    ILogger<AddOrderCommandHandler> logger,
                                    IMapper mapper) : IRequestHandler<AddOrderRequest, AddOrderResponse>
{
    public async Task<AddOrderResponse> Handle(AddOrderRequest addOrderRequest, CancellationToken cancellationToken)
    {
        var order = mapper.Map<Domain.Order>(addOrderRequest);

        var invalidOrderItems = await UpdateOrderItemsAsync(order);
        CalculateOrderTotal(order);
        SetOrderId(order);

        var orderAddress = await GetOrderAddressAsync(addOrderRequest.CustomerAddressId);

        await orderRepository.AddAsync(order);

        return GetAddOrderResponse(order, invalidOrderItems, orderAddress);
    }

    private AddOrderResponse GetAddOrderResponse(Domain.Order order, List<OrderItem> invalidOrderItems, AddOrderAddressResponse addOrderAddressResponse)
    {
        return new AddOrderResponse(GetOrderResponse(order, addOrderAddressResponse), mapper.Map<List<AddOrderInvalidOrderItemResponse>>(invalidOrderItems));
    }

    private AddOrderOrderResponse GetOrderResponse(Domain.Order order, AddOrderAddressResponse addOrderAddressResponse)
    {
        var orderItemsResponse = mapper.Map<List<AddOrderOrderItemResponse>>(order.OrderItems);

        return new AddOrderOrderResponse(order.Id, orderHelper.PaddedOrderNumber(order.OrderNumber),
                                            order.AddressSurname, order.AddressForename, orderItemsResponse,
                                                order.Total, Enums.OrderStatus.Created.ToString(),
                                                    DateOnly.FromDateTime(order.Created).ToString(Constants.DateFormat_ddMMyyyy),
                                                        addOrderAddressResponse);
    }

    private async Task<AddOrderAddressResponse> GetOrderAddressAsync(Guid customerAddressId)
    {
        var customerAddress = await customerAddressService.GetCustomerAddressAsync(customerAddressId);
        if (customerAddress == null)
        {
            logger.LogError("{message}", "Customer address not found for id - {customerAddressId}");
            throw new NotFoundException("Customer address not found.");
        }

        return mapper.Map<AddOrderAddressResponse>(customerAddress);
    }

    public void SetOrderId(Domain.Order order)
    {
        order.Id = Guid.NewGuid();

        foreach (var orderItem in order.OrderItems)
        {
            orderItem.OrderId = order.Id;
        }
    }

    public void CalculateOrderTotal(Domain.Order order)
    {
        order.Total = (decimal)order.OrderItems
                                    .Where(c => c.UnitPrice != null)
                                    .Sum(c => ((double)(c.UnitPrice ?? 0.0m) * c.Quantity));
    }

    private async Task<List<OrderItem>> UpdateOrderItemsAsync(Domain.Order order)
    {
        List<OrderItem> invalidOrderItems = []; ;

        var groupedOrderItemsByProductType = GroupOrderItemsByProductType(order.OrderItems);

        foreach (var orderItems in groupedOrderItemsByProductType)
        {
            var productType = GetProductType(orderItems);

            switch (productType)
            {
                case Helpers.Enums.ProductType.Book:
                    {
                        invalidOrderItems = await UpdateOrderItemsBookDetailsAsync(orderItems, order);
                        break;
                    }
                case Helpers.Enums.ProductType.Music:
                    {
                        break;
                    }
            }
        }

        return invalidOrderItems;
    }

    private static List<List<OrderItem>> GroupOrderItemsByProductType(List<OrderItem> orderItems)
    {
        var result = orderItems.GroupBy(x => x.ProductTypeId)
                               .Select(grp => grp.ToList())
                               .ToList();
        return result;
    }

    private static Enums.ProductType GetProductType(List<OrderItem> orderItems)
    {
        var firstOrderItem = orderItems.FirstOrDefault();
        if (firstOrderItem != null)
        {
            return firstOrderItem.ProductTypeId;
        }

        return Enums.ProductType.NotFound;
    }

    private async Task<List<OrderItem>> UpdateOrderItemsBookDetailsAsync(List<OrderItem> orderItems, Domain.Order order)
    {
        BooksResponse bookDetailsResponse
                    = await bookService.GetBooksDetailsAsync(GetProductIds(orderItems));

        foreach (var bookDetailResponse in bookDetailsResponse.BookResponses)
        {
            UpdateOrderItem(order, bookDetailResponse);
        }

        return RemoveInvalidOrderItemsFromOrder([.. bookDetailsResponse.NotFoundBookResponses], order, Enums.ProductType.Book);
    }

    private static List<OrderItem> RemoveInvalidOrderItemsFromOrder(List<NotFoundBookResponse> notFoundBooks, Domain.Order order, Enums.ProductType productType)
    {
        List<OrderItem> invalidOrderItems = [];

        foreach (var notFoundBook in notFoundBooks)
        {
            OrderItem? invalidOrderItem = order.OrderItems
                                              .Where(o => o.ProductId == new Guid(notFoundBook.Id))
                                              .SingleOrDefault();

            if (invalidOrderItem != null)
            {
                order.OrderItems.Remove(invalidOrderItem);

                invalidOrderItem.ProductTypeId = productType;
                invalidOrderItems.Add(invalidOrderItem);
            }
        }

        return invalidOrderItems;
    }

    private static List<Guid> GetProductIds(List<OrderItem> orderItems)
    {
        return orderItems.Select(c => c.ProductId).ToList();
    }

    private static void UpdateOrderItem(Domain.Order order, BookResponse bookResponse)
    {
        var orderItem = order.OrderItems.SingleOrDefault(o => o.ProductId.Equals(new Guid(bookResponse.Id)));
        if (orderItem != null)
        {
            orderItem.Name = bookResponse.Name;
            orderItem.UnitPrice = decimal.Parse(bookResponse.UnitPrice);
        }
    }
}