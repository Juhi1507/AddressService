using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CustomerService.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(10)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNumber { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderType Gender { get; set; }

        [Required]

        public string PasswordHash { get; set; }

        public string? GitHubId { get; set; }
        public string? GitHubToken { get; set; }

        [Required]
        public string Role { get; set; } = "Customer";
    }

    public enum GenderType
    {
        Male,
        Female,
        Other
    }
}
