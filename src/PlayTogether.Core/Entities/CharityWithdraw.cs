using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class CharityWithdraw : BaseEntity
    {
        public string CharityId { get; set; }
        public Charity Charity { get; set; }

        [Column(TypeName = "float")]
        public float MoneyWithdraw { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
