using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Deposit : BaseEntity
    {
        public string HirerId { get; set; }
        public Hirer HirerDeposit { get; set; }

        [Column(TypeName = "float")]
        public float MoneyDeposit { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
