using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(30, ErrorMessage = "Password must be less than 30 characters")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}