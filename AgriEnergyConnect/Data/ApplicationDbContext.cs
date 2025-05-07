namespace AgriEnergyConnect.Data
{
    using AgriEnergyConnect.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Data
            modelBuilder.Entity<Farmer>().HasData(
                new Farmer { Id = 1, FullName = "Thabo Mokoena", Location = "Free State", ContactInfo = "082 843 2634" },
                new Farmer { Id = 2, FullName = "Anika Jacobs", Location = "Western Cape", ContactInfo = "082 691 0340" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Tomatoes", Category = "Vegetables", ProductionDate = DateTime.Parse("2024-09-01"), FarmerId = 1 },
                new Product { Id = 2, Name = "Wind-Powered Water Pump", Category = "Green Tech", ProductionDate = DateTime.Parse("2025-01-10"), FarmerId = 2 }
            );
        }
    }

}
