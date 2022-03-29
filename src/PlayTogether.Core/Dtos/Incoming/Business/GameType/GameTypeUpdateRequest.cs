using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.GameType
{
    public class GameTypeUpdateRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string ShortName { get; set; }

        [MaxLength(200)]
        public string OtherName { get; set; }

        public string Description {get; set; }
        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}