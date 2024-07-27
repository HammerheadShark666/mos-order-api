namespace Microservice.Order.Api.Mediatr.CompletedOrder.Model;

public class OrderHistory
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; } 
    public string AddressSurname { get; set; } 
    public string AddressForename { get; set; }
    public string OrderNumber { get; set; } 
    public string OrderStatus { get; set; } 
    public List<OrderItemHistory> OrderItems { get; set; } 
    public DateOnly OrderPlaced { get; set; } 
    public decimal Total { get; set; }
    public OrderHistoryAddress Address { get; set; }
}