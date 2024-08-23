namespace Microservice.Order.Api.Mediatr.CompletedOrder.Model;

public class OrderHistory
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string AddressSurname { get; set; } = string.Empty;
    public string AddressForename { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string OrderStatus { get; set; } = string.Empty;
    public List<OrderItemHistory> OrderItems { get; set; } = default!;
    public DateOnly OrderPlaced { get; set; }
    public decimal Total { get; set; }
    public OrderHistoryAddress Address { get; set; } = default!;
}