using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileService.Model
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        [Required, MaxLength(200)]
        public string Street { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(100)]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{5,10}$", ErrorMessage = "Pincode must be between 5 to 10 digits.")]
        public string Pincode { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }
}
