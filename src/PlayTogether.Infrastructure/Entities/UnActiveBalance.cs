using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class UnActiveBalance : BaseEntity
    {
        [MaxLength(100)]
        public string UserBalanceId { get; set; }
        public UserBalance UserBalance { get; set; }

        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        [Column(TypeName = "float")]
        public float Money { get; set; }
        public DateTime DateActive { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
