using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class GameOfPlayer : BaseEntity
    {
        [MaxLength(100)]
        public string GameId { get; set; }
        public Game Game { get; set; }
        
        [MaxLength(50)]
        public string Rank { get; set; }
        
        [MaxLength(100)]
        public string PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
