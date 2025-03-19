using IT15_Trojan_B.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using IT15_Trojan_B.Models;
namespace IT15_Trojan_B.Controllers
{
    public class WorkOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create([FromBody] WorkOrder workOrder)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return Json(new { success = false, errors });
            }

            // Save to database (Example using Entity Framework)
            _context.WorkOrders.Add(workOrder);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetWorkOrders()
        {
            var workOrders = _context.WorkOrders.ToList();
            return Json(workOrders);
        }
    }
}