namespace AgriEnergyConnect.Models
{
    public class Farmer
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Location { get; set; }
        public string? ContactInfo { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
