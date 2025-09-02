using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages(); 

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();        

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Farmer", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    await SeedDataAsync(services);
}

app.Run();

// Seeding database with Farmer and Employee (Microsoft, 2025) (Yu, 2025)
static async Task SeedDataAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed 1st Farmer with products
    var farmerEmail = "farmer@test.com";
    if (await userManager.FindByEmailAsync(farmerEmail) == null)
    {
        var farmerUser = new ApplicationUser
        {
            UserName = farmerEmail,
            Email = farmerEmail,
            FullName = "Test Farmer",
            PhoneNumber = "0123456789"
        };
        await userManager.CreateAsync(farmerUser, "Farmer123!");
        await userManager.AddToRoleAsync(farmerUser, "Farmer");

        var farmerProfile = new Farmer
        {
            ApplicationUserId = farmerUser.Id,
            FullName = farmerUser.FullName,
            ContactInfo = farmerUser.PhoneNumber,
            Location = "123 Farm Lane"
        };

        dbContext.Farmers.Add(farmerProfile);
        await dbContext.SaveChangesAsync(); // Save so the Farmer gets an Id

        // Add two products for this farmer
        dbContext.Products.AddRange(
            new Product
            {
                FarmerId = farmerProfile.Id,
                Name = "Organic Tomatoes",
                Category = "Fresh Produce",
                Description = "No pesticides or injections",
                ProductionDate = DateTime.Parse("2025-03-05")
            },
            new Product
            {
                FarmerId = farmerProfile.Id,
                Name = "Free-range Eggs",
                Category = "Livestock & Animal Products",
                Description = "High quality free range eggs maintained with highest standards",
                ProductionDate= DateTime.Parse("2024-08-22"),
            }
        );
    }

    // Seed 2nd Farmer with Products
    var farmerEmail2 = "farmer@test2.com";
    if (await userManager.FindByEmailAsync(farmerEmail2) == null)
    {
        var farmerUser = new ApplicationUser
        {
            UserName = farmerEmail2,
            Email = farmerEmail2,
            FullName = "Second Test Farmer",
            PhoneNumber = "0123456789"
        };
        await userManager.CreateAsync(farmerUser, "Farmer123!");
        await userManager.AddToRoleAsync(farmerUser, "Farmer");

        var farmerProfile2 = new Farmer
        {
            ApplicationUserId = farmerUser.Id,
            FullName = farmerUser.FullName,
            ContactInfo = farmerUser.PhoneNumber,
            Location = "Farmland avenue 69"
        };

        dbContext.Farmers.Add(farmerProfile2);
        await dbContext.SaveChangesAsync(); // Save so the Farmer gets an Id

        // Add products for this farmer
        dbContext.Products.AddRange(
            new Product
            {
                FarmerId = farmerProfile2.Id,
                Name = "Organic Raw Honey",
                Category = "Organic Products",
                Description = "100% pure, unprocessed honey from organic bee farms.",
                ProductionDate = DateTime.Parse("2025-04-10")
            },
            new Product
            {
                FarmerId = farmerProfile2.Id,
                Name = "Fresh Baby Spinach",
                Category = "Fresh Produce",
                Description = "Crisp baby spinach leaves harvested daily from local farms.",
                ProductionDate = DateTime.Parse("2025-05-01")
            },
            new Product
            {
                FarmerId = farmerProfile2.Id,
                Name = "Free-range Chicken Thighs",
                Category = "Poultry",
                Description = "Juicy chicken thighs from healthy, free-roaming birds.",
                ProductionDate = DateTime.Parse("2025-04-25")
            },
            new Product
            {
                FarmerId = farmerProfile2.Id,
                Name = "Dried Chickpeas",
                Category = "Legumes & Nuts",
                Description = "High-quality, sun-dried chickpeas perfect for cooking or grinding.",
                ProductionDate = DateTime.Parse("2025-03-15")
            },
            new Product
            {
                FarmerId = farmerProfile2.Id,
                Name = "Whole Grain Brown Rice",
                Category = "Grains & Cereals",
                Description = "Nutrient-rich brown rice, unpolished and chemical-free.",
                ProductionDate = DateTime.Parse("2025-02-20")
            }

        );
    }

    // Seed Employee
    var employeeEmail = "employee@test.com";
    if (await userManager.FindByEmailAsync(employeeEmail) == null)
    {
        var employeeUser = new ApplicationUser
        {
            UserName = employeeEmail,
            Email = employeeEmail,
            FullName = "Test Employee",
            PhoneNumber = "0987654321"
        };
        await userManager.CreateAsync(employeeUser, "Employee123!");
        await userManager.AddToRoleAsync(employeeUser, "Employee");

        dbContext.Employees.Add(new Employee
        {
            ApplicationUserId = employeeUser.Id,
            FullName = employeeUser.FullName,
            ContactInfo = employeeUser.PhoneNumber
        });
    }

    await dbContext.SaveChangesAsync();
}
