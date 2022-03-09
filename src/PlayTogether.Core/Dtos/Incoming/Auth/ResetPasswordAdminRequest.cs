using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class ResetPasswordAdminRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is required")]
        [Compare("NewPassword",
            ErrorMessage = "New password and confirmation new password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}