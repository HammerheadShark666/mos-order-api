using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Order.Api.Domain;

[Table("MSOS_OrderStatus")]
public class OrderStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Api.Helpers.Enums.OrderStatus Id { get; set; }

    [MaxLength(75)]
    [Required]
    public string Status { get; set; }
}