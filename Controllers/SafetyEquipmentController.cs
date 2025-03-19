using IT15_Trojan_B.Data;
using IT15_Trojan_B.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class SafetyEquipmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public SafetyEquipmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var safetyEquipments = await _context.SafetyEquipments.ToListAsync();
        return View(safetyEquipments);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SafetyEquipment safetyEquipment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(safetyEquipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(safetyEquipment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SafetyEquipment safetyEquipment)
    {
        if (id != safetyEquipment.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.Update(safetyEquipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(safetyEquipment);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var safetyEquipment = await _context.SafetyEquipments.FindAsync(id);
        if (safetyEquipment != null)
        {
            _context.SafetyEquipments.Remove(safetyEquipment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
