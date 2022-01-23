using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerUpdateInfoRequest
    {
        public string Avatar { get; set; }

        [Required]        
        public string Firstname { get; set; }

        [Required]  
        public string Lastname { get; set; }

        [Required]  
        public string City { get; set; }

        [Required]  
        public DateTime DateOfBirth { get; set; }
        
        [Required]  
        public string Description { get; set; }
        
        [Required]  
        public bool Gender { get; set; }
    }
}