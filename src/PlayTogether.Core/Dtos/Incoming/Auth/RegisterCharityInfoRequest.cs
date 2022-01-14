using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterCharityInfoRequest: RegisterRequest
    {
        [Required]
        [MaxLength(100, ErrorMessage = "OrganizationName must less than 100 characters")]
        public string OrganizationName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
