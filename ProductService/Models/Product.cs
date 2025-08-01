using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0 and 100.")]
        public double DiscountPercentage { get; set; } = 0; // ✅ New field for discount

        [NotMapped] // ✅ Prevents storing in DB
        public decimal DiscountedPrice => Price - (Price * (decimal)(DiscountPercentage / 100));
    }
}
