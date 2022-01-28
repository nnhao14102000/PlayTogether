using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Game
{
    public class GameGetByIdResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public IList<TypeOfGameResponseForGame> TypeOfGames { get; set; }

        // list rank
    }
}