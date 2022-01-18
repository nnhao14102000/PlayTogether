namespace PlayTogether.Infrastructure.Entities
{
    public class Report : BaseEntity
    {
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public Player PlayerMakeReport { get; set; }
        public string PlayerMakeReportId { get; set; }

        public Hirer HirerReceiveReport { get; set; }
        public string HirerReceiveReportId { get; set; }

        public string ReportMessage { get; set; }
        public bool IsApprove { get; set; }
    }
}
