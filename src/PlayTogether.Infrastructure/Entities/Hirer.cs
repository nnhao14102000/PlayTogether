using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Hirer : BaseEntity
    {
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [MaxLength(50)]
        public string Nickname { get; set; }

        [DataType("date")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string Gender { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [DataType("float")]
        public float Balance { get; set; }

        public string Description { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
