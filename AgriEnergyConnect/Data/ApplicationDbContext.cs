using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor accepting DbContext options and passing them to the base IdentityDbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // DbSets represent the tables in the database
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }

        // Configure entity relationships and constraints using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Farmer to ApplicationUser relationship (1-to-1 or many-to-1 depending on usage)
            modelBuilder.Entity<Farmer>()
                .HasOne(f => f.ApplicationUser)          
                .WithMany()                               
                .HasForeignKey(f => f.ApplicationUserId)   
                .OnDelete(DeleteBehavior.Cascade);         

            // Configure Product to Farmer relationship (many-to-1)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Farmer)                    
                .WithMany(f => f.Products)              
                .HasForeignKey(p => p.FarmerId)            
                .OnDelete(DeleteBehavior.Cascade);         

            // Configure Employee to ApplicationUser relationship
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.ApplicationUser)          
                .WithMany()                                
                .HasForeignKey(e => e.ApplicationUserId)   
                .OnDelete(DeleteBehavior.Cascade);         
        }
    }
}
