using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect.Models.ViewModel
{
    public class ProductFilterViewModel
    {
        public List<Product> Products { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Category")]
        public string ProductType { get; set; }

        public List<string> ProductTypes { get; set; }
    }
}
