using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class PlayerWithdraw : BaseEntity
    {
        [MaxLength(100)]
        public string PlayerId { get; set; }
        public Player Player { get; set; }

        [Column(TypeName = "float")]
        public float MoneyWithdraw { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
