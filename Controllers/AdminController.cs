using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IT15_Trojan_B.Data;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace IT15_Trojan_B.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var logs = await _context.SecurityLogs.OrderByDescending(log => log.Timestamp).ToListAsync();
            return View(logs);
        }

        public IActionResult WorkOrder()
        {
            return View();
        }

        public IActionResult Tools()
        {
            return View();
        }

        public IActionResult Safety()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
