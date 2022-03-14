﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Rating : BaseEntity
    {
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        //public User FromUser { get; set; }
        [MaxLength(100)]
        public string FromUserId { get; set; }

        //public User ToUser { get; set; }
        [MaxLength(100)]
        public string ToUserId { get; set; }

        public string Comment { get; set; }
        
        [Column(TypeName = "float")]
        public float Rate { get; set; }
        public bool IsViolate { get; set; }
        public bool IsActive { get; set; }
    }
}
    