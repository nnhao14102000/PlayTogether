using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class TypeOfGame : BaseEntity
    {
        public GameType GameType { get; set; }
        [MaxLength(100)]
        public string GameTypeId { get; set; }

        public Game Game { get; set; }
        [MaxLength(100)]
        public string GameId { get; set; }
    }
}
