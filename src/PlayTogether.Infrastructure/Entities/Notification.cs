using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Notification : BaseEntity
    {
        [MaxLength(100)]
        public string ReceiverId { get; set; }

        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(1000)]
        public string Message { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
    }
}