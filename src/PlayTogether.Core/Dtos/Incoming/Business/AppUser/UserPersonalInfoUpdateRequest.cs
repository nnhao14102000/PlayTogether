using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.AppUser
{
    public class UserPersonalInfoUpdateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public bool Gender { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }

        public DateTime UpdateDate = DateTime.Now;
    }

}