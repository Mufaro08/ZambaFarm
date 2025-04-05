using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZambaFarm.Models; // your project namespace
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;


namespace ZambaFarm.Controllers
{
    //[Authorize(Roles = "Admin")]  // Ensuring only Admins have access
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminController> _logger;

        // Constructor to inject dependencies
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // List of all users in the system
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            return View(users); // passing users to view
        }

        // Action to assign a role to a user
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{roleName}' assigned to {user.UserName}.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error assigning role.";
            }

            return RedirectToAction("Index");
        }

        // Create a user manually (e.g., Admin for testing)
        public async Task<IActionResult> CreateAdminUser()
        {
            var user = new IdentityUser { UserName = "muf@gmail.com", Email = "muf@gmail.com" };
            var result = await _userManager.CreateAsync(user, "Admin@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = "Admin user created!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error creating admin user.";
            }

            return RedirectToAction("Index");
        }
    }
}
