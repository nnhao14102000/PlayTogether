using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class CharityWithdraw : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string CharityId { get; set; }
        public Charity Charity { get; set; }

        [Column(TypeName = "float")]
        [Required]
        public float MoneyWithdraw { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
