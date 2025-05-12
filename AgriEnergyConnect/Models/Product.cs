using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        public string Description { get; set; }


        [Required]
        public DateTime ProductionDate { get; set; }

        // Foreign key to Farmer
        [Required]
        public int FarmerId { get; set; }

        [ForeignKey(nameof(FarmerId))]
        public Farmer Farmer { get; set; }
    }
}
