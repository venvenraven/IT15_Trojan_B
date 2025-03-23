using IT15_Trojan_B.Data;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IT15_Trojan_B.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProjectController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] Project project)
        {
            if (project == null)
            {
                Console.WriteLine("Received null project data.");
                return Json(new { success = false, message = "Invalid data received." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("User is not authenticated.");
                return Unauthorized();
            }

            try
            {
                project.Id = Guid.NewGuid().ToString();
                project.UserId = userId;
                project.Status = "Pending";

                Console.WriteLine($"Saving project: {project.ProjectName}, User: {userId}");

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                Console.WriteLine("Project saved successfully!");

                return Json(new { success = true });
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database Error: " + dbEx.InnerException?.Message);
                return Json(new { success = false, message = "Database Error: " + dbEx.InnerException?.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                Console.WriteLine("User is not authenticated.");
                return Unauthorized();
            }

            var userProjects = await _context.Projects
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    projectName = p.ProjectName,
                    jobDescription = p.JobDescription,
                    address = p.Address,
                    deadline = p.Deadline.ToString("yyyy-MM-dd"),
                    status = p.Status
                })
                .ToListAsync();

            Console.WriteLine($"Projects found: {userProjects.Count}");

            return Json(userProjects);
        }

    }
}
