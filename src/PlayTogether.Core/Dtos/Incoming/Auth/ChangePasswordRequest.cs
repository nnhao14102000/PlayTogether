using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class ChangePasswordRequest
    {
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is required")]
        [Compare("NewPassword",
            ErrorMessage = "New password and confirmation new password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}