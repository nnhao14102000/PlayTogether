using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Game
{
    public class GameUpdateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string DisplayName { get; set; }

        [MaxLength(200)]
        public string OtherName { get; set; }

        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}