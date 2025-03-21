using IT15_Trojan_B.Data;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")] // Ensure only Admins can access
public class SecurityLogsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SecurityLogsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

   [HttpPost]
public async Task<IActionResult> LogSecurityEvent(string action)
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return BadRequest("User not found.");

    var roles = await _userManager.GetRolesAsync(user);
    string userRole = roles.Any() ? string.Join(", ", roles) : "No Role"; // Avoid NULL issues

    var securityLog = new SecurityLog
    {
        UserName = user.UserName,
        UserRole = userRole, // Ensure this is assigned!
        Action = action,
        IPAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
        Timestamp = DateTime.UtcNow
    };

    _context.SecurityLogs.Add(securityLog);
    await _context.SaveChangesAsync();

    return Ok(new { message = "Security event logged successfully." });
}


    [HttpGet]
    public async Task<IActionResult> GetSecurityLogs()
    {
        var logs = await _context.SecurityLogs
            .OrderByDescending(log => log.Timestamp)
            .ToListAsync();
        return Json(logs);
    }
}
