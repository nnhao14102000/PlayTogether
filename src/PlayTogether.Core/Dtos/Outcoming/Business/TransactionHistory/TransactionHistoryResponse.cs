namespace PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory
{
    public class TransactionHistoryResponse
    {
        public string Id { get; set; }
        public string Operation { get; set; }
        public float Money { get; set; }
        public string TypeOfTransaction { get; set; }
        public string ReferenceTransactionId { get; set; }
    }
}