using IT15_Trojan_B.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using IT15_Trojan_B.Hubs;


namespace TrojanBuilders.Controllers
{
    public class MaterialsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IHubContext<MaterialsHub> _hubContext;
        public MaterialsController(ApplicationDbContext context, IHubContext<MaterialsHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            var materials = _context.Materials.ToList();
            return View("~/Views/Admin/Materials.cshtml", materials);
        }

        public async Task<IActionResult> NotifyClients()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "New materials update!");
            return Ok();
        }

        // ✅ Add or Update Material (Merged Create & Update)
        
        [HttpPost]
        public async Task<IActionResult> SaveMaterial([FromBody] Material material)
        {
            if (material == null)
                return Json(new { success = false, message = "Invalid material data" });

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return Json(new { success = false, message = "Invalid data", errors });
            }

            if (material.Id == 0)
            {
                // Create new material
                _context.Materials.Add(material);
            }
            else
            {
                // Update existing material
                var existingMaterial = await _context.Materials.FindAsync(material.Id);
                if (existingMaterial == null)
                    return Json(new { success = false, message = "Material not found" });

                // Update only necessary fields
                existingMaterial.Name = material.Name;
                existingMaterial.Quantity = material.Quantity;
                existingMaterial.Brand = material.Brand;
                existingMaterial.Supplier = material.Supplier;
                existingMaterial.Price = material.Price;
                existingMaterial.Status = material.Status;
                existingMaterial.StorageLocation = material.StorageLocation;
                existingMaterial.DateAcquired = material.DateAcquired;
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("RefreshMaterials");

            return Json(new { success = true, id = material.Id, material });
        }



        // ✅ Delete Material
        [HttpPost]
        public async Task<IActionResult> ArchiveMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
                return Json(new { success = false, message = "Material not found" });

            material.Status = "Archived"; // Assuming there's a status column
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("RefreshMaterials");

            return Json(new { success = true });
        }



    }
}