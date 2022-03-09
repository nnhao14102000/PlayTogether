using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class ResetPasswordTokenRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}