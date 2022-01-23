using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Player
{
    public class UpdatePlayerInfoRequest
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

        public DateTime UpdateDate = DateTime.Now;
    }
}