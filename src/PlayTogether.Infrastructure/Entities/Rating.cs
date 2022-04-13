using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Rating : BaseEntity
    {
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        //public User ToUser { get; set; }
        [MaxLength(100)]
        public string ToUserId { get; set; }

        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rate { get; set; }
        public bool IsViolate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApprove { get; set; }
    }
}
