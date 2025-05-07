using AgriEnergyConnect.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers.Employee
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public EmployeeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
    }
}
