using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterCharityInfoDto: RegisterDto
    {
        [Required(ErrorMessage = "Organization name is required")]
        [MaxLength(30, ErrorMessage = "Organization name must be less than 30 letters")]
        public string OrganizationName { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}
