using AutoMapper;
using MediatR;
using Microservice.Order.Api.Data.Repository.Interfaces;
using Microservice.Order.Api.Domain;
using Microservice.Order.Api.Grpc.Interfaces;
using Microservice.Order.Api.Helpers;
using Microservice.Order.Api.Helpers.Exceptions;
using Microservice.Order.Api.Protos;

namespace Microservice.Order.Api.MediatR.AddOrder;

public class AddOrderCommandHandler(IOrderRepository orderRepository,
                                    IBookService bookService,
                                    ICustomerAddressService customerAddressService,
                                    IMapper mapper) : IRequestHandler<AddOrderRequest, AddOrderResponse>
{
    private IOrderRepository _orderRepository { get; set; } = orderRepository;
    private IBookService _bookService { get; set; } = bookService;
    private ICustomerAddressService _customerAddressService { get; set; } = customerAddressService;
    private IMapper _mapper { get; set; } = mapper;

    public async Task<AddOrderResponse> Handle(AddOrderRequest addOrderRequest, CancellationToken cancellationToken)
    { 
        var order = _mapper.Map<Domain.Order>(addOrderRequest);

        var invalidOrderItems = await UpdateOrderItemsAsync(order);  
        CalculateOrderTotal(order);
        SetOrderId(order);

        await _orderRepository.AddAsync(order);

        return await GetAddOrderResponse(order, invalidOrderItems);
    } 

    private async Task<AddOrderResponse> GetAddOrderResponse(Domain.Order order, List<OrderItem> invalidOrderItems)
    {
        return new AddOrderResponse(await GetOrderResponse(order), _mapper.Map<List<AddOrderInvalidOrderItemResponse>>(invalidOrderItems));
    }

    private async Task<AddOrderOrderResponse> GetOrderResponse(Domain.Order order)
    {
        var orderItemsResponse = _mapper.Map<List<AddOrderOrderItemResponse>>(order.OrderItems);
        var addressResponse = await GetOrderAddress(order.CustomerAddressId);
          
        return new AddOrderOrderResponse(order.Id, OrderHelper.PaddedOrderNumber(order.OrderNumber),
                                            order.AddressSurname, order.AddressForename, orderItemsResponse,
                                                order.Total, Enums.OrderStatus.Created.ToString(), 
                                                    DateOnly.FromDateTime(order.Created).ToString(Constants.DateFormat_ddMMyyyy),
                                                        addressResponse);
    } 

    private async Task<AddOrderAddressResponse> GetOrderAddress(Guid customerAddressId)
    {
        var customerAddress = await _customerAddressService.GetCustomerAddressAsync(customerAddressId);
        if (customerAddress == null)
            throw new NotFoundException($"Customer address not found for id - {customerAddressId}");

        return _mapper.Map<AddOrderAddressResponse>(customerAddress);
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
        order.Total = (decimal)order.OrderItems.Where(c => c.UnitPrice != null).Sum(c => (double)c.UnitPrice * c.Quantity);
    }

    private async Task<List<OrderItem>> UpdateOrderItemsAsync(Domain.Order order)
    {
        List<OrderItem> invalidOrderItems = new List<OrderItem>();

        var groupedOrderItemsByProductType = GroupOrderItemsByProductType(order.OrderItems);
          
        foreach(var orderItems in groupedOrderItemsByProductType)
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

    private List<List<OrderItem>> GroupOrderItemsByProductType(List<OrderItem> orderItems)
    {  
        var result = orderItems.GroupBy(x => x.ProductTypeId)
                               .Select(grp => grp.ToList())
                               .ToList();
        return result; 
    }

    private Enums.ProductType GetProductType(List<OrderItem> orderItems) {

        var firstOrderItem = orderItems.FirstOrDefault();
        if(firstOrderItem != null)
        {
            return firstOrderItem.ProductTypeId;
        }

        return Enums.ProductType.NotFound;
    }
     
    private async Task<List<OrderItem>> UpdateOrderItemsBookDetailsAsync(List<OrderItem> orderItems, Domain.Order order)
    { 
        BooksResponse bookDetailsResponse 
                    = await _bookService.GetBooksDetailsAsync(GetProductIds(orderItems));

        foreach (var bookDetailResponse in bookDetailsResponse.BookResponses)
        {
            UpdateOrderItem(order, bookDetailResponse, Enums.ProductType.Book); 
        } 

        return RemoveInvalidOrderItemsFromOrder(bookDetailsResponse.NotFoundBookResponses.ToList(), order, Enums.ProductType.Book);
    }

    private List<OrderItem> RemoveInvalidOrderItemsFromOrder(List<NotFoundBookResponse> notFoundBooks, Domain.Order order, Enums.ProductType productType)
    {
        List<OrderItem> invalidOrderItems = new List<OrderItem>();

        foreach (var notFoundBook in notFoundBooks)
        {
            OrderItem invalidOrderItem = order.OrderItems.Where(o => o.ProductId == new Guid(notFoundBook.Id)).SingleOrDefault();

            if (invalidOrderItem != null)
            {
                order.OrderItems.Remove(invalidOrderItem);

                invalidOrderItem.ProductTypeId = productType;
                invalidOrderItems.Add(invalidOrderItem);
            }
        }

        return invalidOrderItems;
    }
     
    private List<Guid> GetProductIds(List<OrderItem> orderItems)
    {
        return orderItems.Select(c => c.ProductId).ToList();
    }
      
    private void UpdateOrderItem(Domain.Order order, BookResponse bookResponse, Enums.ProductType productType)
    {
        var orderItem = order.OrderItems.SingleOrDefault(o => o.ProductId.Equals(new Guid(bookResponse.Id)));
        if (orderItem != null)
        { 
            orderItem.Name = bookResponse.Name;
            orderItem.UnitPrice = decimal.Parse(bookResponse.UnitPrice);
        }
    }
} 