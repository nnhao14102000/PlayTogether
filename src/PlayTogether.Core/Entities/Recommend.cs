using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class Recommend : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [Range(1, 100)]
        public int UserAge { get; set; }

        public bool UserGender { get; set; }

        [MaxLength(100)]
        public string GameOrderId { get; set; }


        [Required]
        [MaxLength(100)]
        public string PlayerId { get; set; }

        [Range(1, 100)]
        public int PlayerAge { get; set; }

        public bool PlayerGender { get; set; }

        [MaxLength(100)]
        public string GameOfPlayerId { get; set; }


        [Column(TypeName = "float")]
        public float Rate { get; set; }
    }
}