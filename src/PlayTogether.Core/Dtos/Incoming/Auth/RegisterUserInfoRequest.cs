using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterUserInfoRequest: RegisterRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Firstname must less than 50 characters")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Lastname must less than 50 characters")]
        public string Lastname { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        [Compare("true", ErrorMessage = "Please confirm email")]
        public bool ConfirmEmail { get; set; }
    }
}
