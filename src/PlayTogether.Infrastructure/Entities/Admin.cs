using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Admin : BaseEntity
    {
        [MaxLength(100)]
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
