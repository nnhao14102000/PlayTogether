using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame
{
    public class TypeOfGameCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string GameTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string GameId { get; set; }
        
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}