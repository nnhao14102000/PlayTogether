using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class SystemConfig : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int NO { get; set; }

        [Required]
        [Column(TypeName = "float")]
        public float Value { get; set; }
    }
}