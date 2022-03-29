using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameOfUser
{
    public class GameOfUserCreateRequest
    {

        [Required]
        public string GameId { get; set; }
        
        public string RankId { get; set; }

        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}