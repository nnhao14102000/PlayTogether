using System;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Player
{
    public class PlayerGetByIdResponseForPlayer
    {
        public string Id { get; set; }
        
        public string Avatar { get; set; }
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string City { get; set; }

        public DateTime DateOfBirth { get; set; }

        public ICollection<ImagesOfPlayer> Images { get; set; }
        
        public string Description { get; set; }
        
        public bool Gender { get; set; }
    }
}