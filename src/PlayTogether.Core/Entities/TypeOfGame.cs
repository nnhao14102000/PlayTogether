using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class TypeOfGame : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string GameTypeId { get; set; }
        public GameType GameType { get; set; }

        [Required]
        [MaxLength(100)]
        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}
