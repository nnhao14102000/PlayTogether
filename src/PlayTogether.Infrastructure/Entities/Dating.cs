using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Dating : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Range(0,23)]
        public int FromHour { get; set; }
        [Range(0, 23)]
        public int ToHour { get; set; }

        public bool IsMON { get; set; }
        public bool IsTUE { get; set; }
        public bool IsWED { get; set; }
        public bool IsTHU { get; set; }
        public bool IsFRI { get; set; }
        public bool IsSAT { get; set; }
        public bool IsSUN { get; set; }
    }
}
