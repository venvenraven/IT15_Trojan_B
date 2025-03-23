using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using IT15_Trojan_B.Data; // Adjust this according to your project
using IT15_Trojan_B.Models;

namespace IT15_Trojan_B.Controllers
{
    [Authorize]
    public class BillingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BillingController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Fetch Invoices for the Logged-in User
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var userId = _userManager.GetUserId(User);
            var invoices = await _context.Invoices
                .Where(i => i.CustomerId == userId)
                .Select(i => new
                {
                    InvoiceNumber = i.InvoiceNumber,
                    Date = i.Date.ToString("yyyy-MM-dd"),
                    CustomerName = i.CustomerName,
                    Address = i.Address,
                    Email = i.Email,
                    WorkOrder = i.WorkOrder,
                    ProjectName = i.ProjectName,
                    AssignedWorker = i.AssignedWorker,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Total = i.TotalAmount,
                    DueDate = i.DueDate.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            return Json(invoices);
        }

        // POST: Simulate Payment Processing
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int invoiceId)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null)
            {
                return Json(new { success = false, message = "Invoice not found." });
            }

            invoice.IsPaid = true;
            invoice.PaymentDate = DateTime.Now;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Payment successful!" });
        }
    }
}
