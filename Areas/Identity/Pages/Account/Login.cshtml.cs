﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using IT15_Trojan_B.Data; // Added for ApplicationDbContext
using IT15_Trojan_B.Models; // Added for SecurityLog

namespace IT15_Trojan_B.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly ApplicationDbContext _context; // Added ApplicationDbContext

        public LoginModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger,
            ApplicationDbContext context) // Injected ApplicationDbContext
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context; // Assigned context
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        [ValidateAntiForgeryToken] // Added CSRF Protection
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    // Get the user's IP address
                    string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                    // Get user roles
                    var userRoles = await _context.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .ToListAsync();

                    string userRole = userRoles.Any() ? string.Join(", ", userRoles) : "Unknown";

                    // Create a new security log entry
                    var log = new SecurityLog
                    {
                        UserName = user.UserName,
                        UserRole = userRole,
                        Action = "User Logged In",
                        IPAddress = ipAddress,
                        Timestamp = DateTime.UtcNow
                    };

                    // Save the log entry to the database
                    _context.SecurityLogs.Add(log);
                    await _context.SaveChangesAsync();

                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin")) return LocalRedirect("~/Admin/Dashboard");
                    if (roles.Contains("Employee")) return LocalRedirect("~/Home/EmployeeDashboard");
                    if (roles.Contains("Customer")) return LocalRedirect("~/Home/CustomerDashboard");

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    ViewData["IsLockedOut"] = true;
                    TempData["LockoutMessage"] = "Too many failed login attempts. Your account has been locked for 10 minutes.";

                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page();
        }
    }
}