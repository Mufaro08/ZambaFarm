using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ZambaFarm.Models
{
    public class users
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
