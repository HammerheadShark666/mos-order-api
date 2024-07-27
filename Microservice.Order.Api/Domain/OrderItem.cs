using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Order.Api.Domain;

[Table("MSOS_OrderItem")]
public class OrderItem
{
    [Key]
    [ForeignKey("OrderId")]
    public Guid OrderId { get; set; } 

    [Key]
    public Guid ProductId { get; set; }

    [MaxLength(150)] 
    public string Name { get; set; }

    public Helpers.Enums.ProductType ProductTypeId { get; set; }

    [ForeignKey(nameof(ProductTypeId))]
    public ProductType ProductType { get; set; } 

    [Required]
    [Range(1, 999, ErrorMessage = "{0} must be between {1:c} and {2:c}")]
    public int Quantity { get; set; }
     
    [Column(TypeName = "decimal(19, 2)")]
    [Range(0.00, 9999, ErrorMessage = "{0} must be between {1:c} and {2:c}")]
    public decimal? UnitPrice { get; set; }
}