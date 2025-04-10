using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZambaFarm.Data;
using ZambaFarm.Models;

var builder = WebApplication.CreateBuilder(args);

// Get connection strings
var defaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var farmContextConnectionString = builder.Configuration.GetConnectionString("FarmContext")
    ?? throw new InvalidOperationException("Connection string 'FarmContext' not found.");

// Configure database contexts
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(defaultConnectionString));
builder.Services.AddDbContext<FarmContext>(options =>
    options.UseSqlServer(farmContextConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
    .AddRoles<IdentityRole>() // Enable role management
    .AddEntityFrameworkStores<FarmContext>();

// Configure Identity cookie settings for login
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";  // Set the login path
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";  // Set the access denied path
});

// Register Razor Pages (important for Identity)
builder.Services.AddRazorPages();  // Add this line to register Razor Pages services.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Create roles on application start-up
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    var roleNames = new[] { "Admin", "User", "Manager" }; // Example roles

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Configure the HTTP request pipeline
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
app.UseRouting();

// Authentication and Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

// Map Razor Pages (ensure Identity routes work)
app.MapRazorPages();  // Ensure Razor Pages is mapped here.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();