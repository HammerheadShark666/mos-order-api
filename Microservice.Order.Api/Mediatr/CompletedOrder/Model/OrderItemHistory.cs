namespace Microservice.Order.Api.Mediatr.CompletedOrder.Model;

public class OrderItemHistory
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string ProductType { get; set; }
    public decimal? UnitPrice { get; set; }
    public int Quantity { get; set; }
}
