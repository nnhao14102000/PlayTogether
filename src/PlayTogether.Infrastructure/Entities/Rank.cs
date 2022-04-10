using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Rank : BaseEntity
    {
        [Required]
        public int NO { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}