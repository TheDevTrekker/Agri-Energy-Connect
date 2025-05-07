namespace AgriEnergyConnect.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }

        // Optional: navigation properties to profiles
        public Farmer? FarmerProfile { get; set; }
        public Employee? EmployeeProfile { get; set; }
    }
}
