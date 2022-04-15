using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.TransactionHistory
{
    public class DepositRequest
    {
        [Required]
        public string MomoTransactionId { get; set; }
        [Required]
        public float Money { get; set; }
    }
}