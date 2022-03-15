using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class UserBalance : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        [Column(TypeName = "float")]
        public float ActiveBalance { get; set; }
        public ICollection<UnActiveBalance> UnActiveBalances { get; set; }
    }
}
