using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Game : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Rank> Ranks { get; set; }
        public IList<GameOfPlayer> GamesOfPlayers { get; set; }
    }
}