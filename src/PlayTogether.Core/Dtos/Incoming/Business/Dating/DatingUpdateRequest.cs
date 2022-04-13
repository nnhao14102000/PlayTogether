using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Dating
{
    public class DatingUpdateRequest
    {
        [Range(0, 1440)]
        [Required]
        public int FromHour { get; set; }
        
        [Range(0, 1440)]
        [Required]
        public int ToHour { get; set; }

        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);

    }
}