using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Ignore : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public string IgnoreUserId { get; set; }
        public int TimeIgnore { get; set; }
        public bool IsForever { get; set; }
        public bool IsActive { get; set; }
    }
}
