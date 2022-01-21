using System.Collections.Generic;

namespace PlayTogether.Infrastructure.Entities
{
    public class GameType : BaseEntity
    {
        public string TypeName { get; set; }
        public IList<TypeOfGame> TypeOfGames { get; set; }
    }
}
