using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class UserBalance : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        [Column(TypeName = "float")]
        public float ActiveBalance { get; set; }
        public ICollection<UnActiveBalance> UnActiveBalances { get; set; }
        public ICollection<TransactionHistory> TransactionHistories { get; set; }
    }
}
