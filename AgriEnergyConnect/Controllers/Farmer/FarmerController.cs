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

        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        // Product POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(string Name, string Category, string Description, DateTime ProductionDate)
        {
            var user = await _userManager.GetUserAsync(User);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.ApplicationUserId == user.Id);

            if (farmer == null)
                return NotFound();

            var product = new Product
            {
                Name = Name,
                Category = Category,
                Description = Description,
                ProductionDate = ProductionDate,
                FarmerId = farmer.Id
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = await _context.Products
                .Include(p => p.Farmer)
                .FirstOrDefaultAsync(p => p.Id == id && p.Farmer.ApplicationUserId == user.Id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

    }
}
