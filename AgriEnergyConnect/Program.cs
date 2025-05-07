using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages(); 

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Farmer", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

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
    await SeedDataAsync(services); 
}

app.Run();

// Seeding database with Farmer and Employee
static async Task SeedDataAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

    // Seed Farmer
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

        dbContext.Farmers.Add(new Farmer
        {
            ApplicationUserId = farmerUser.Id,
            FullName = farmerUser.FullName,
            ContactInfo = farmerUser.PhoneNumber
        });
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
