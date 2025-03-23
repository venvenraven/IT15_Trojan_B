using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using IT15_Trojan_B.Models;
using IT15_Trojan_B.ViewModels;
using IT15_Trojan_B.Data;

namespace IT15_Trojan_B.Controllers
{
    public class EmployeeRegisterController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;  // ✅ Changed to IdentityUser
        private readonly SignInManager<IdentityUser> _signInManager;  // ✅ Changed to IdentityUser
        private readonly ApplicationDbContext _context; // Inject DbContext

        public EmployeeRegisterController(UserManager<IdentityUser> userManager,
                                          SignInManager<IdentityUser> signInManager,
                                          ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(EmployeeRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the form errors and try again.";
                return View(model);
            }

            string filePath = "uploads/no-file.png"; // Default file path

            // Process file upload if a file is selected
            if (model.UploadFile != null && model.UploadFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadFile.FileName;
                filePath = Path.Combine("uploads", uniqueFileName); // Save relative path

                string fullPath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.UploadFile.CopyToAsync(stream);
                }
            }

            var user = new IdentityUser  // ✅ Use IdentityUser instead of ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Insert into Employees table
                var employee = new Employee
                {
                    FullName = model.FullName,
                    Address = model.Address,
                    ContactNumber = model.ContactNumber,
                    Email = model.Email,  // Link to IdentityUser email
                    Classification = model.Classification,
                    Specialty = model.Specialty,
                    IsActive = true,
                    IsAssigned = false,
                    UploadedFilePath = filePath // Save the file path
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["SuccessMessage"] = "Employee registered successfully!";
                return RedirectToAction("Register");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            TempData["ErrorMessage"] = "Registration failed. Please check the errors.";
            return View(model);
        }
    }
}
