using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterAdminInfoRequest: RegisterRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name must less than 50 characters")]
        public string Name { get; set; }
    }
}
