using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using IT15_Trojan_B.Data; // Added for ApplicationDbContext
using IT15_Trojan_B.Models; // Added for SecurityLog

namespace IT15_Trojan_B.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly ApplicationDbContext _context; // Added for Security Logs

        public LogoutModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILogger<LogoutModel> logger, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(User); // Get current user
            string redirectUrl = "/"; // Default redirect to Landing Page (Homepage)

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                string userRole = roles.Any() ? string.Join(", ", roles) : "No Role"; // Fix: Ensure a default value

                _logger.LogInformation($"User Roles: {userRole}");

                if (roles.Contains("Admin"))
                {
                    redirectUrl = "/Identity/Account/Login"; // Ensure leading '/'
                }

                // Get IP address
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                // Log security event
                var log = new SecurityLog
                {
                    UserName = user.UserName,
                    UserRole = userRole, // Ensure UserRole is stored properly
                    Action = "User Logged Out",
                    IPAddress = ipAddress,
                    Timestamp = DateTime.UtcNow
                };

                _context.SecurityLogs.Add(log);
                await _context.SaveChangesAsync();
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            return LocalRedirect(redirectUrl); // Ensures safe redirect
        }
    }
}
