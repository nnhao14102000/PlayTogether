using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Music : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        public IList<MusicOfPlayer> MusicOfPlayers { get; set; }
    }
}