using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Charity : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string OrganizationName { get; set; }

        public string Avatar { get; set; }

        public string Information { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Donate> Donates { get; set; }
        public ICollection<CharityWithdraw> CharityWithdraws { get; set; }
    }
}
