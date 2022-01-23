using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerProfileResponse
    {
        public string Id { get; set; }
        
        public string Avatar { get; set; }

        public string Email { get; set; }
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string City { get; set; }

        public DateTime DateOfBirth { get; set; }
        
        public float Balance { get; set; }
    }
}