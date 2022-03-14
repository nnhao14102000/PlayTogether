using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Infrastructure.Entities
{
    public class Recommend : BaseEntity
    {
        [MaxLength(100)]
        public string HirerId { get; set; }
        //public User User { get; set; }

        [Range(1, 100)]
        public int HirerAge { get; set; }

        public bool HirerGender { get; set; }

        [MaxLength(100)]
        public string GameOrderId { get; set; }
        public Game GameOrder { get; set; }


        [MaxLength(100)]
        public string PlayerId { get; set; }
        //public User OrderUser { get; set; }

        [Range(1, 100)]
        public int PlayerAge { get; set; }

        public bool PlayerGender { get; set; }

        [MaxLength(100)]
        public string GameOfPlayerId { get; set; }
        public Game GameOfPlayer { get; set; }


        [Column(TypeName = "float")]
        public float Rate { get; set; }
    }
}