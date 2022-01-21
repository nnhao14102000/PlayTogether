using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Order : BaseEntity
    {
        public string PlayerId { get; set; }
        public Player Player { get; set; }

        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }

        //public Rating Rating { get; set; }
        //public Report Report { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Report> Reports { get; set; }

        public DateTime TimeStart { get; set; }

        public int TotalTimes { get; set; }

        [Column(TypeName = "float")]
        public float TotalPrices { get; set; }
        public string Status { get; set; }
        public bool IsDonate => Donate != null;
        public Donate Donate { get; set; }
    }
}
