using System.ComponentModel.DataAnnotations;

namespace DeveloperStore.Sales.API.Models
{
    public class CreateProductRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
