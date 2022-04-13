using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class BehaviorPoint : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Range(0, 100)]
        public int Point { get; set; }

        [Range(0, 100)]
        public int SatisfiedPoint { get; set; }
        public ICollection<BehaviorHistory> BehaviorHistories { get; set; }
    }
}
