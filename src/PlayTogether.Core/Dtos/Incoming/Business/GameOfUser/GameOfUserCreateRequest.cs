using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameOfUser
{
    public class GameOfUserCreateRequest
    {

        [Required]
        [MaxLength(100)]
        public string GameId { get; set; }
        
        [MaxLength(100)]
        public string RankId { get; set; }

        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}