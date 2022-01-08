using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterUserInfoDto: RegisterDto
    { 
        [Required(ErrorMessage = "Firstname is required")]
        [MaxLength(30, ErrorMessage = "Firstname must be less than 30 letters")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [MaxLength(30, ErrorMessage = "Lastname must be less than 30 letters")]
        public string  Lastname { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}
