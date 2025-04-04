using Microsoft.AspNetCore.Identity;
namespace ZambaFarm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
