using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class AuthCredentials
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
