using System.Collections.Generic;
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

        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}