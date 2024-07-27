using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservice.Order.Api.Domain;

[Table("MSOS_ProductType")]
public class ProductType
{ 
     [Key]
    public Api.Helpers.Enums.ProductType Id { get; set; }

    [MaxLength(75)]
    [Required]
    public string Name { get; set; }
}
