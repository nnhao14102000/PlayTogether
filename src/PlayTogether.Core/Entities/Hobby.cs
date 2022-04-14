using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Entities
{
    public class Hobby : BaseEntity
    {
        [MaxLength(100)]
        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [MaxLength(100)]
        [Required]
        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}