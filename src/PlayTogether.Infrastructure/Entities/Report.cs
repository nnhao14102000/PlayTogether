using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Report : BaseEntity
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

        public string ReportMessage { get; set; }
        public bool? IsApprove { get; set; }
    }
}
