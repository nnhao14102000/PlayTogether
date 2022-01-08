using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterAdminInfoDto: RegisterDto
    {
        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}
