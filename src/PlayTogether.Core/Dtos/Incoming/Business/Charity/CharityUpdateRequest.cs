using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Charity
{
    public class CharityUpdateRequest
    {
        [MaxLength(50)]
        [Required]
        public string OrganizationName { get; set; }

        public string Avatar { get; set; }

        [Required]
        public string Information { get; set; }

        [MaxLength(200)]
        [Required]
        public string Address { get; set; }

        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; }
        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}