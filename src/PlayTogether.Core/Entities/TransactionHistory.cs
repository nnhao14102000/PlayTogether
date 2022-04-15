using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class TransactionHistory : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserBalanceId { get; set; }
        public UserBalance UserBalance { get; set; }

        [Required]
        [MaxLength(10)]
        public string Operation { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Money { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string TypeOfTransaction { get; set; }
        public string ReferenceTransactionId { get; set; }
        // public string Status { get; set; }
    }
}
