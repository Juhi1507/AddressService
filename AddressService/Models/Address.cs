using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AddressService.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        [Required]
        public int CustomerID { get; set; } // Foreign key reference to Customer

        [Required]
        public string FlatNo { get; set; }

        [Required, MaxLength(200)]
        public string Street { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }

        [Required, MaxLength(50)]
        public string State { get; set; }

        [Required, MaxLength(10)]
        [RegularExpression(@"^\d{5,10}$", ErrorMessage = "Invalid ZIP Code.")]
        public string ZipCode { get; set; }

        [Required, MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AddressType Type { get; set; } // Enum for Home, Work, etc.
    }

    public enum AddressType
    {
        Home,
        Work,
        Other
    }
}
