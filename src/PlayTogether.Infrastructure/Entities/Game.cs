using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Game : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(50)]
        public string DisplayName { get; set; }

        [MaxLength(200)]
        public string OtherName { get; set; }

        public ICollection<Rank> Ranks { get; set; }
        public IList<GameOfUser> GameOfUsers { get; set; }
        public IList<TypeOfGame> TypeOfGames { get; set; }
        public IList<Hobby> Hobbies { get; set; }
    }
}