using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Ignore : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [MaxLength(100)]
        public string IgnoreUserId { get; set; }
        public int TimeIgnore { get; set; }
        public bool IsForever { get; set; }
        public bool IsActive { get; set; }
    }
}
