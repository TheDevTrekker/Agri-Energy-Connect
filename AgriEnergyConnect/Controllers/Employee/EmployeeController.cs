using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers.Employee
{
    // Controller for handling employee-related actions such as managing farmers and viewing products
    public class EmployeeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor injecting logger, database context, and user manager services
        public EmployeeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        // Displays a list of all farmers and their associated products
        public async Task<IActionResult> Dashboard()
        {
            var farmers = await _context.Farmers
                .Include(f => f.Products)
                .ToListAsync();

            if (farmers == null)
            {
                // If no farmers found, show error view
                return View("Error");
            }

            return View(farmers);
        }

        // GET: /Employee/AddFarmer
        // Displays the form to add a new farmer
        public IActionResult AddFarmer()
        {
            return View();
        }

        // POST: /Employee/AddFarmer
        // Handles the submission of the form to create a new farmer and associated user account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(CreateFarmerViewModel model)
        {
            // Check if form input is valid
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check the form for errors.";
                return View(model);
            }

            // Ensure email is unique
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "A user with this email already exists.";
                return View(model);
            }

            // Create the identity user for authentication
            var farmerUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };

            // Attempt to create the user with the provided password
            var result = await _userManager.CreateAsync(farmerUser, model.Password);
            if (!result.Succeeded)
            {
                // Display errors if creation fails
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                }
                return View(model);
            }

            // Assign the new user to the "Farmer" role
            await _userManager.AddToRoleAsync(farmerUser, "Farmer");

            // Create a corresponding Farmer profile in the database
            var farmerProfile = new AgriEnergyConnect.Models.Farmer
            {
                ApplicationUserId = farmerUser.Id,
                FullName = model.FullName,
                ContactInfo = model.PhoneNumber,
                Location = model.Address
            };

            // Save farmer profile to the database
            _context.Farmers.Add(farmerProfile);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Farmer successfully added!";
            return RedirectToAction("AddFarmer");  // Redirect to form again or choose another action
        }

        // Displays a view for filtering and selecting products by farmer
        public async Task<IActionResult> ViewProducts()
        {
            // Retrieve all farmers and populate dropdown for filtering
            var farmers = await _context.Farmers.ToListAsync();
            ViewBag.Farmers = new SelectList(farmers, "Id", "FullName");

            return View();
        }

        // Retrieves filtered list of products based on farmer and optional filters like date range and product type
        public async Task<IActionResult> GetProducts(int farmerId, DateTime? startDate, DateTime? endDate, string productType)
        {
            // Get the selected farmer and include their products
            var farmer = await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.Id == farmerId);

            if (farmer == null)
            {
                return NotFound(); // Return 404 if farmer not found
            }

            // Start with all products and apply filters
            var products = farmer.Products.AsQueryable();

            if (startDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(productType))
            {
                products = products.Where(p => p.Category == productType);
            }

            // Return a partial view with the filtered product list
            return PartialView("_ProductList", products.ToList());
        }
    }
}