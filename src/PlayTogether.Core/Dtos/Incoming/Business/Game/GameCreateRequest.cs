using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Game
{
    public class GameCreateRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string DisplayName { get; set; }

        [MaxLength(200)]
        public string OtherName { get; set; }

        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}