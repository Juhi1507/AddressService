using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletService.Models
{
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WalletID { get; set; }

        [Required]
        public int CustomerID { get; set; } // Foreign key reference to Customer

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative.")]
        public decimal Balance { get; set; }
    }
}
