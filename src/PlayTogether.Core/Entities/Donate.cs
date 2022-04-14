using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class Donate : BaseEntity
    {
        public AppUser User { get; set; }
        [MaxLength(100)]
        public string UserId { get; set; }

        public Charity Charity { get; set; }
        [MaxLength(100)]
        public string CharityId { get; set; }

        [Column(TypeName = "float")]
        public float Money { get; set; }
    }
}
