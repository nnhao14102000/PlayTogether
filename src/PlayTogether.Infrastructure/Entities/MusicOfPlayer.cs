using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class MusicOfPlayer : BaseEntity
    {   
        [MaxLength(100)]
        public string MusicId { get; set; }
        public Music Music { get; set; }

        [MaxLength(100)]
        public string PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
