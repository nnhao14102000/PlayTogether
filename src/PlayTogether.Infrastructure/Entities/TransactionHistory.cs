namespace PlayTogether.Infrastructure.Entities
{
    public class TransactionHistory : BaseEntity
    {
        public string UserBalanceId { get; set; }
        public UserBalance UserBalance { get; set; }
        public string Operation { get; set; }
        public float Money { get; set; }
        public string TypeOfTransaction { get; set; }
        public string ReferenceTransactionId { get; set; }
        // public string Status { get; set; }
    }
}
