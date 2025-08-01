
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemID { get; set; }

        [Required]
        public int OrderID { get; set; } // Foreign key reference to Order

        [Required]
        public int ProductID { get; set; } // Foreign key reference to Product

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
