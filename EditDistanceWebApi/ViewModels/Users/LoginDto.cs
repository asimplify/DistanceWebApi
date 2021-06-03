using System.ComponentModel.DataAnnotations;

namespace DistanceWebApi.ViewModels.Users
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}