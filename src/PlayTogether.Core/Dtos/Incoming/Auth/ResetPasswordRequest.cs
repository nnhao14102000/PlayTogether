using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class ResetPasswordRequest : ResetPasswordAdminRequest
    {
        [Required]
        public string Token { get; set; }
    }
}