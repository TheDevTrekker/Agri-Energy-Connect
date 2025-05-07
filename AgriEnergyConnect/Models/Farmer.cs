using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public string? Location { get; set; }

        public string? ContactInfo { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
