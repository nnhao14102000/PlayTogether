using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame
{
    public class TypeOfGameCreateRequest
    {
        [Required]
        public string GameTypeId { get; set; }

        [Required]
        public string GameId { get; set; }
        
        public DateTime CreatedDate = DateTime.Now;
    }
}