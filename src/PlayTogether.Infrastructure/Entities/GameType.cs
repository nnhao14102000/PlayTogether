using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class GameType : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string ShortName { get; set; }

        [MaxLength(200)]
        public string OtherName { get; set; }
        public string Description {get; set; }
        public IList<TypeOfGame> TypeOfGames { get; set; }
    }
}
