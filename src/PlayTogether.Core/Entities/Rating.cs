using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Rating : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        //public User ToUser { get; set; }
        [Required]
        [MaxLength(100)]
        public string ToUserId { get; set; }

        [MaxLength(200)]
        public string Comment { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rate { get; set; }
        public bool IsViolate { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApprove { get; set; }
    }
}
