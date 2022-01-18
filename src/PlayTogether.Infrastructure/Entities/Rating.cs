namespace PlayTogether.Infrastructure.Entities
{
    public class Rating : BaseEntity
    {
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public Player PlayerReceiveFeedback { get; set; }
        public string PlayerReceiveFeedbackId { get; set; }

        public Hirer HirerMakeFeedback { get; set; }
        public string HirerMakeFeedbackId { get; set; }

        public string Comment { get; set; }
        public float Rate { get; set; }
        public bool IsViolate { get; set; }
    }
}
