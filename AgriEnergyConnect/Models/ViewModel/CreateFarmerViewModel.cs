using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models.ViewModel
{
    public class CreateFarmerViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string? Address { get; set; } // Optional field
    }
}
