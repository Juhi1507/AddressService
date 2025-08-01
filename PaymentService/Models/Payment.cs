using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentService.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        [Required]
        public int OrderID { get; set; } // Foreign key reference to Order

        [Required]
        public int CustomerID { get; set; } // Foreign key reference to Customer

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } // Wallet or COD

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed

        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
