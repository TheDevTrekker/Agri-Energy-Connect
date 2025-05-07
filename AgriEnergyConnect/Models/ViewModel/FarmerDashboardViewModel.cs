namespace AgriEnergyConnect.Models.ViewModel
{
    public class FarmerDashboardViewModel
    {
        public Farmer Farmer { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
