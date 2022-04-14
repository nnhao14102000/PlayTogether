using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Notification : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string ReceiverId { get; set; }

        [MaxLength(500)]
        [Required]
        public string Title { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Message { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
        public string ReferenceLink { get; set; }
    }
}