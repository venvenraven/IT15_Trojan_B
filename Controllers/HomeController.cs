using System.Diagnostics;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using IT15_Trojan_B.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // For IdentityUser
using System.Threading.Tasks;

namespace IT15_Trojan_B.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = user?.GetType().GetProperty("FullName")?.GetValue(user, null) as string;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeDashboard()
        {
            ViewBag.TotalEmployees = _context.Employees.Count();
            ViewBag.ActiveEmployees = _context.Employees.Where(u => u.IsActive).Count();
            ViewBag.AvailableEmployees = _context.Employees.Where(u => !u.IsAssigned).Count();

            ViewBag.ClassificationData = new int[]
            {
                _context.Employees.Count(e => e.Classification == "General Contractor"),
                _context.Employees.Count(e => e.Classification == "Subcontractor"),
                _context.Employees.Count(e => e.Classification == "Site Supervisor")
            };

            ViewBag.SpecialtyData = new int[]
            {
                _context.Employees.Count(e => e.Specialty == "Electrical"),
                _context.Employees.Count(e => e.Specialty == "Plumbing"),
                _context.Employees.Count(e => e.Specialty == "Carpentry"),
                _context.Employees.Count(e => e.Specialty == "Finish")
            };

            return View();
        }

        public IActionResult CustomerProfile()
        {
            return View();
        }

        public IActionResult EmployeeProfile()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
