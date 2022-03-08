using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Order : BaseEntity
    {
        [MaxLength(100)]
        public string PlayerId { get; set; }
        public Player Player { get; set; }

        [MaxLength(100)]
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Report> Reports { get; set; }

        public DateTime TimeStart { get; set; }
        
        public int TotalTimes { get; set; }

        [Column(TypeName = "float")]
        public float TotalPrices { get; set; }

        [MaxLength(100)]
        public string Message { get; set; }
        public string Status { get; set; }

        public DateTime ProcessExpired {get; set; }
        public DateTime TimeFinish {get; set; }
        
        public Donate Donate { get; set; }
    }
}
