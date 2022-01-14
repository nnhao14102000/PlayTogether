using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterAdminInfoRequest: RegisterRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Firstname must less than 50 characters")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Lastname must less than 50 characters")]
        public string Lastname { get; set; }
    }
}
