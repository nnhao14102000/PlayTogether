﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Deposit : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Column(TypeName = "float")]
        public float MoneyDeposit { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
