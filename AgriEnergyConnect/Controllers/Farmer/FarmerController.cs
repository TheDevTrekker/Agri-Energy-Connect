using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect.Controllers.Farmer
{
    public class FarmerController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
