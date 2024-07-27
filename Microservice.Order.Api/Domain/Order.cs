using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Order.Api.Domain;

[Table("MSOS_Order")]
public class Order
{
    [Key]
    public Guid Id { get; set; }
     
    public Guid CustomerId { get; set; }

    public Guid CustomerAddressId { get; set; }

    [Required]
    [StringLength(50)]
    public string AddressSurname { get; set; }

    [Required]
    [StringLength(50)]
    public string AddressForename { get; set; }

    public int OrderNumber {  get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public Helpers.Enums.OrderStatus OrderStatusId { get; set; }

    [ForeignKey(nameof(OrderStatusId))]
    public OrderStatus OrderStatus { get; set; } 

    [Required]
    [Column(TypeName = "decimal(19, 2)")]
    [Range(0.01, 99999, ErrorMessage = "{0} must be between {1:c} and {2:c}")]
    public decimal Total { get; set; }

    [Required]
    public DateTime Created { get; set; } = DateTime.Now;

    [Required]
    public DateTime LastUpdated { get; set; } = DateTime.Now;
}
