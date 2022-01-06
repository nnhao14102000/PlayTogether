using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(30, ErrorMessage = "Username must be less than 30 letters")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 letters")]
        [MaxLength(30, ErrorMessage = "Password must be less than 30 letters")]
        public string Password { get; set; }
    }
}