using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterUserInfoRequest : RegisterRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name must less than 50 characters")]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        [Range(minimum: 1, maximum: 1, ErrorMessage = "Please verify email")]
        public bool ConfirmEmail { get; set; }
    }
}
