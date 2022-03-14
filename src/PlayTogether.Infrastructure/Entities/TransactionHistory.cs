namespace PlayTogether.Infrastructure.Entities
{
    public class TransactionHistory : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Operation { get; set; }
        public float Money { get; set; }
        public string TypeOfTransaction { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
    }
}
