using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class SystemFeedback : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        [MaxLength(1000)]
        public string Message { get; set; }
        [MaxLength(100)]
        public string TypeOfFeedback { get; set; }
    }
}
