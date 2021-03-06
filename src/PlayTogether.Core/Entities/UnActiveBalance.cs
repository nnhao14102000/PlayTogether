using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class UnActiveBalance : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string UserBalanceId { get; set; }
        public UserBalance UserBalance { get; set; }

        [MaxLength(100)]
        [Required]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        [Column(TypeName = "float")]
        [Required]
        public float Money { get; set; }
        public DateTime DateActive { get; set; }
        public bool IsRelease { get; set; } = false;
    }
}
