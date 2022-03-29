using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Music
{
    public class MusicUpdateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}