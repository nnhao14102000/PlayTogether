using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.TransactionHistory
{
    public class DepositRequest
    {
        [Required]
        [MaxLength(100)]
        public string MomoTransactionId { get; set; }
        [Required]
        [Range(0, float.MaxValue)]
        public float Money { get; set; }
    }
}