using IT15_Trojan_B.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;
using IT15_Trojan_B.Hubs;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 🔹 Configure Identity with security settings
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 12;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 3;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 🔹 Secure Cookies & Authentication Paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// 🔹 Register Email Service (Prevent Email Errors)
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSignalR();


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaPage("Identity", "/Account/Manage");
    });

var app = builder.Build();

// 🔹 Apply Migrations & Seed Roles/Users Securely
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        context.Database.Migrate(); // ✅ Auto-run migrations
        SeedRolesAndUsers(roleManager, userManager).GetAwaiter().GetResult(); // ✅ Ensure it's run synchronously
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// 🔹 Configure Middleware (Ensure Correct Order)
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // ✅ Correct order

app.UseAuthentication();
app.UseAuthorization();

// 🔹 Route Configuration
app.UseEndpoints(endpoints =>
{
    // Admin Dashboard Route
    endpoints.MapControllerRoute(
     name: "admin",
     pattern: "admin/dashboard",
     defaults: new { controller = "Admin", action = "Dashboard" });


    // Employee Dashboard Route
    endpoints.MapControllerRoute(
        name: "employeeDashboard",
        pattern: "employee/dashboard",
        defaults: new { controller = "Home", action = "EmployeeDashboard" });

    // Customer Dashboard Route
    endpoints.MapControllerRoute(
        name: "customerDashboard",
        pattern: "customer/dashboard",
        defaults: new { controller = "Home", action = "CustomerDashboard" });

    // Default Route
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Work Order Route
    endpoints.MapControllerRoute(
        name: "workorder",
        pattern: "workorder/{action=WorkOrder}/{id?}",
        defaults: new { controller = "Admin" });

    // Materials Route
    endpoints.MapControllerRoute(
        name: "materials",
        pattern: "materials/{action=Index}/{id?}",
        defaults: new { controller = "Materials" });

    // Tools Route
    endpoints.MapControllerRoute(
        name: "tools",
        pattern: "tools/{action=Tools}/{id?}",
        defaults: new { controller = "Admin" });

    // Safety Equipment Route
    endpoints.MapControllerRoute(
        name: "safetyEquipment",
        pattern: "safetyequipment/{action=Index}/{id?}",
        defaults: new { controller = "SafetyEquipment" });

    // SignalR Hub Route
    endpoints.MapHub<MaterialsHub>("/materialsHub");
    
    endpoints.MapRazorPages(); // ✅ Ensure Razor Pages are mapped
});

app.Run();

// 🔹 Role & User Seeding Method (Run Synchronously)
async Task SeedRolesAndUsers(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    string[] roleNames = { "Admin", "Employee", "Customer" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // 🔹 Secure Admin User Creation
    var adminEmail = "admin@trojanbuilders.com";
    var adminPassword = "Admin@Secure123"; // ✅ Strong Password
    await CreateUserIfNotExists(userManager, adminEmail, adminPassword, "Admin");

    // 🔹 Secure Employee User Creation
    var employeeEmail = "employee@trojanbuilders.com";
    var employeePassword = "Employee@Secure123"; // ✅ Strong Password
    await CreateUserIfNotExists(userManager, employeeEmail, employeePassword, "Employee");
}

// 🔹 Helper Function for Secure User Creation
async Task CreateUserIfNotExists(UserManager<IdentityUser> userManager, string email, string password, string role)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        var newUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true // ✅ Required for login security
        };

        var result = await userManager.CreateAsync(newUser, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, role);
        }
    }
}

// 🔹 Fake Email Sender Implementation (Prevents Email Errors)
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask; // ✅ Avoids email-related errors
    }
}
