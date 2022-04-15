using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Chat : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Required]
        [MaxLength(100)]
        public string ReceiveId { get; set; }

        [MaxLength(500)]
        [Required]
        public string Message { get; set; }
        
        public bool IsActive { get; set; }
    }
}