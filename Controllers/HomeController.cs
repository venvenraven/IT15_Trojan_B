using System.Diagnostics;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using IT15_Trojan_B.Data;
using Microsoft.AspNetCore.Authorization; // Ensure this is your DbContext namespace

namespace IT15_Trojan_B.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
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

        [Authorize(Roles = "Customer")]
        public IActionResult CustomerDashboard()
        {
            var model = new CustomerDashboardViewModel
            {
                PendingOrders = _context.Orders.Count(o => o.Status == "Pending"),
                CompletedOrders = _context.Orders.Count(o => o.Status == "Completed"),
                UpcomingAppointments = _context.Appointments.Count(a => a.Date >= DateTime.Now),
                TotalPayments = _context.Payments.Sum(p => p.Amount)
               
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
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
