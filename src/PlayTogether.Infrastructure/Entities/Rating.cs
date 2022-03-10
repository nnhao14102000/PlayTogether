using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Rating : BaseEntity
    {
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public Player Player { get; set; }
        [MaxLength(100)]
        public string PlayerId { get; set; }

        public Hirer Hirer { get; set; }
        [MaxLength(100)]
        public string HirerId { get; set; }

        public string Comment { get; set; }
        
        [Column(TypeName = "float")]
        public float Rate { get; set; }
        public bool IsViolate { get; set; }
        public bool IsActive { get; set; }
    }
}
    