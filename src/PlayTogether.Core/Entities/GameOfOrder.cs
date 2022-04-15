using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class GameOfOrder : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        [MaxLength(100)]
        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}
