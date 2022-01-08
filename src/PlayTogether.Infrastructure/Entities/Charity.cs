﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Charity : BaseEntity
    {
        public string IdentityId { get; set; }

        [MaxLength(50)]
        public string OrganizationName { get; set; }

        public string Avatar { get; set; }

        public string Description { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }


        [MaxLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "float")]
        public float Balance { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
