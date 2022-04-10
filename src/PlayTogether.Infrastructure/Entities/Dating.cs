using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Dating : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Range(0, 23)]
        [Required]
        public int FromHour { get; set; }

        [Range(0, 23)]
        [Required]
        public int ToHour { get; set; }

        [Range(2, 8)]
        [Required]
        public int DayInWeek { get; set; }
    }
}
