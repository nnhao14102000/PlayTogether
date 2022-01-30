using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Music
{
    public class MusicCreateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime CreatedDate = DateTime.Now;
    }
}