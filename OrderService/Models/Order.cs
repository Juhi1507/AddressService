using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderService.Models;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [Required]
        public int CustomerID { get; set; } // Foreign key reference to Customer

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // Wallet or COD

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Shipped, Delivered, Cancelled

        public DateTime? EstimatedDeliveryDate { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
