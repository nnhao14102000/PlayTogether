using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.MusicOfPlayer
{
    public class MusicOfPlayerCreateRequest
    {
        [Required]
        public string MusicId { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}