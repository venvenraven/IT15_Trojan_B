using IT15_Trojan_B.Data;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ToolEquipmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public ToolEquipmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var tools = await _context.ToolEquipments.ToListAsync(); // Fetch data
        if (tools == null)
        {
            tools = new List<ToolEquipment>(); // Prevent null reference
        }
        return View(tools);
    }

    [HttpPost]
    public async Task<IActionResult> AddToolEquipment([FromBody] ToolEquipment toolEquipment)
    {
        if (ModelState.IsValid)
        {
            _context.ToolEquipments.Add(toolEquipment);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    [HttpPost]
    public async Task<IActionResult> EditToolEquipment([FromBody] ToolEquipment toolEquipment)
    {
        if (ModelState.IsValid)
        {
            _context.ToolEquipments.Update(toolEquipment);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    [HttpPost]
    public async Task<IActionResult> ArchiveToolEquipment(int id)
    {
        var toolEquipment = await _context.ToolEquipments.FindAsync(id);
        if (toolEquipment != null)
        {
            _context.ToolEquipments.Remove(toolEquipment);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }
}
