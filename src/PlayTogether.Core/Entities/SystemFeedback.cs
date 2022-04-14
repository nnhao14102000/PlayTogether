using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class SystemFeedback : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TypeOfFeedback { get; set; }
        public bool? IsApprove { get; set; }
    }
}
