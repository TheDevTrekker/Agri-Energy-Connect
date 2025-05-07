using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect.Data
{
    public class IdentitySeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Farmer", "Employee" };

            // Seed roles
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed a test Farmer user
            var farmerEmail = "farmer@example.com";
            if (await userManager.FindByEmailAsync(farmerEmail) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = farmerEmail,
                    Email = farmerEmail,
                    FullName = "Seeded Farmer"
                };
                var result = await userManager.CreateAsync(user, "P@ssword1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Farmer");
                }
            }

            // Seed a test Employee user
            var employeeEmail = "employee@example.com";
            if (await userManager.FindByEmailAsync(employeeEmail) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    FullName = "Seeded Employee"
                };
                var result = await userManager.CreateAsync(user, "P@ssword1!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                }
            }
        }
    }
}

