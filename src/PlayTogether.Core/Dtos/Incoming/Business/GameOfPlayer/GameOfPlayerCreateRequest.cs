using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer
{
    public class GameOfPlayerCreateRequest
    {

        [Required]
        public string GameId { get; set; }
        
        public string Rank { get; set; }

        public DateTime CreatedDate = DateTime.Now;
    }
}