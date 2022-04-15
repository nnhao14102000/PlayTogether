using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Report : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        //public User FromUser { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        //public User ToUser { get; set; }
        [Required]
        [MaxLength(100)]
        public string ToUserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string ReportMessage { get; set; }
        public bool? IsApprove { get; set; }
    }
}
