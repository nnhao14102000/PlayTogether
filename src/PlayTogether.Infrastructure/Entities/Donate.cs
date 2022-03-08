using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Donate : BaseEntity
    {
        public Order Order { get; set; }
        [MaxLength(100)]
        public string OrderId { get; set; }

        public Player Player { get; set; }
        [MaxLength(100)]
        public string PlayerId { get; set; }

        public Charity Charity { get; set; }
        [MaxLength(100)]
        public string CharityId { get; set; }
    }
}
