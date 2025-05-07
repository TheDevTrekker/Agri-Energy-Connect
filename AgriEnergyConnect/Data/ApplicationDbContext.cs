using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships
            modelBuilder.Entity<Farmer>()
                .HasOne(f => f.ApplicationUser)
                .WithMany()
                .HasForeignKey(f => f.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Farmer)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FarmerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed Data (Note: ApplicationUserId should reference real seeded Identity users)
            modelBuilder.Entity<Farmer>().HasData(
                new Farmer
                {
                    Id = 1,
                    FullName = "Thabo Mokoena",
                    Location = "Free State",
                    ContactInfo = "082 843 2634",
                    ApplicationUserId = null // Assign valid user ID if seeding users too
                },
                new Farmer
                {
                    Id = 2,
                    FullName = "Anika Jacobs",
                    Location = "Western Cape",
                    ContactInfo = "082 691 0340",
                    ApplicationUserId = null
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Tomatoes",
                    Category = "Vegetables",
                    ProductionDate = DateTime.Parse("2024-09-01"),
                    FarmerId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Wind-Powered Water Pump",
                    Category = "Green Tech",
                    ProductionDate = DateTime.Parse("2025-01-10"),
                    FarmerId = 2
                }
            );
        }
    }
}
