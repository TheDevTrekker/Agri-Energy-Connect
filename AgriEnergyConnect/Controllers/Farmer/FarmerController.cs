using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using AgriEnergyConnect.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers.Farmer
{
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FarmerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers
                .Include(f => f.Products)
                .FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);

            if (farmer == null)
            {
                return NotFound();
            }

            var viewModel = new FarmerDashboardViewModel
            {
                Farmer = farmer,
                Products = farmer.Products.ToList()
            };

            return View(viewModel);
        }

        // Product POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(string Name, string Category, DateTime ProductionDate)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);

            if (farmer == null)
                return NotFound();

            var product = new Product
            {
                Name = Name,
                Category = Category,
                ProductionDate = ProductionDate,
                FarmerId = farmer.Id
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }
    }
}
