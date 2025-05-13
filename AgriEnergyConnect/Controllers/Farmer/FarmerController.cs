using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers.Farmer
{
    // This controller handles actions related to farmers, such as viewing the dashboard and managing products
    public class FarmerController : Controller
    {
        // Dependency-injected database context for accessing the application's data
        private readonly ApplicationDbContext _context;

        // UserManager to manage application users (e.g., get currently logged-in user)
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor injecting the required services
        public FarmerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Displays the farmer dashboard with their profile and associated products
        public async Task<IActionResult> Dashboard()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the farmer entity along with their products using eager loading
            var farmer = await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);

            // Return 404 if no farmer is found for the user
            if (farmer == null)
            {
                return NotFound();
            }

            // Prepare the view model with the farmer and their products
            var viewModel = new FarmerDashboardViewModel
            {
                Farmer = farmer,
                Products = farmer.Products.ToList()
            };

            // Pass the view model to the view
            return View(viewModel);
        }

        // Displays the form for adding a new product
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        // Handles the form submission for adding a new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(string Name, string Category, string Description, DateTime ProductionDate)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the farmer entity for the current user
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);

            if (farmer == null)
                return NotFound();

            // Create and populate the product entity
            var product = new Product
            {
                Name = Name,
                Category = Category,
                Description = Description,
                ProductionDate = ProductionDate,
                FarmerId = farmer.Id
            };

            // Add the new product to the database and save changes
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Redirect back to the dashboard after adding the product
            return RedirectToAction("Dashboard");
        }

        // Handles deletion of a product by the farmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            // Find the product matching the given ID that belongs to the current farmer
            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(p => p.Id == id && p.Farmer.ApplicationUserId == user.Id);

            // Return 404 if the product is not found or does not belong to the user
            if (product == null)
            {
                return NotFound();
            }

            // Remove the product and save changes
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            // Redirect back to the dashboard after deletion
            return RedirectToAction("Dashboard");
        }
    }
}
