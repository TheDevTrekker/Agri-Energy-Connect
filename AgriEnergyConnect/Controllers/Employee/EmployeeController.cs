using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers.Employee
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Dashboard()
        {
            var farmers = await _context.Farmers
                .Include(f => f.Products)
                .ToListAsync();
            if (farmers == null)
            {
                // Handle the case where no farmers are found.
                return View("Error");
            }

            return View(farmers);
        }

        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(CreateFarmerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please check the form for errors.";
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "A user with this email already exists.";
                return View(model);
            }

            // Create Identity user
            var farmerUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(farmerUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                }
                return View(model);
            }

            await _userManager.AddToRoleAsync(farmerUser, "Farmer");

            // Create Farmer profile
            var farmerProfile = new AgriEnergyConnect.Models.Farmer
            {
                ApplicationUserId = farmerUser.Id,
                FullName = model.FullName,
                ContactInfo = model.PhoneNumber,
                Location = model.Address
            };

            _context.Farmers.Add(farmerProfile);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Farmer successfully added!";
            return RedirectToAction("AddFarmer");  // Redirect to the AddFarmer view or elsewhere
        }

    }
}
