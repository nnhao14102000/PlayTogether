﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Deposit : BaseEntity
    {
        [MaxLength(100)]
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }

        [Column(TypeName = "float")]
        public float MoneyDeposit { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
