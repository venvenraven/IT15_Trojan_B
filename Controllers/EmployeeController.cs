using Microsoft.AspNetCore.Mvc;

namespace IT15_Trojan_B.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Profile page
        public IActionResult Profile()
        {
            return View();
        }
    }
}