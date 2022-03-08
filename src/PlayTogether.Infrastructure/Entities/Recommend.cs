using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Recommend : BaseEntity
    {
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }

        public string PlayerId { get; set; }
        public Player Player { get; set; }
        
        [Column(TypeName = "float")]
        public float Rating { get; set; }
        public TimeSpan Timestamp { get; set; }
    }
}