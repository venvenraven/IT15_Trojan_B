using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IT15_Trojan_B.Data;
using IT15_Trojan_B.Models;

namespace IT15_Trojan_B.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
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

    }
}