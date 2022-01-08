﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Player: BaseEntity
    {
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string Firstname { get; set; }

        [MaxLength(50)]
        public string Lastname { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        public bool Gender { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "float")]
        public float PricePerHour { get; set; }

        [Column(TypeName = "float")]
        public float Rating { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; } = true;

        [MaxLength(50)]
        public string Status { get; set; }
    }
}
