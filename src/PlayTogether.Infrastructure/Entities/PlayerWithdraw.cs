using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class PlayerWithdraw : BaseEntity
    {
        public string PlayerId { get; set; }
        public Player Player { get; set; }

        [Column(TypeName = "float")]
        public float MoneyWithdraw { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
