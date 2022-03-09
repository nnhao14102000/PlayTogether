using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Recommend : BaseEntity
    {
        [MaxLength(100)]
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }

        [MaxLength(100)]
        public string PlayerId { get; set; }
        public Player Player { get; set; }
        
        [Column(TypeName = "float")]
        public float Rating { get; set; }
    }
}