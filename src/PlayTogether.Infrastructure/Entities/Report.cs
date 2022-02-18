namespace PlayTogether.Infrastructure.Entities
{
    public class Report : BaseEntity
    {
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public Player Player { get; set; }
        public string PlayerId { get; set; }

        public Hirer Hirer { get; set; }
        public string HirerId { get; set; }

        public string ReportMessage { get; set; }
        public bool IsApprove { get; set; }
    }
}
