using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class Donate : BaseEntity
    {
        public AppUser User { get; set; }
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }

        public Charity Charity { get; set; }
        [Required]
        [MaxLength(100)]
        public string CharityId { get; set; }

        [Column(TypeName = "float")]
        [Required]
        public float Money { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
    }
}
