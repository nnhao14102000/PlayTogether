using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Hobby : BaseEntity
    {
        [MaxLength(100)]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [MaxLength(100)]
        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}