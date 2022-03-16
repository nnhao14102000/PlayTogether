using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Chat : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [MaxLength(100)]
        public string ReceiveId { get; set; }

        [MaxLength(500)]
        [Required]
        public string Message { get; set; }
        
        public bool IsActive { get; set; }
    }
}