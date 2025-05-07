namespace AgriEnergyConnect.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FullName { get; set; }
    }
}
