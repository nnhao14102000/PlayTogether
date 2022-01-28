using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.GameType
{
    public class GameTypeGetByIdResponse
    {
        public string Id { get; set; }
        public string TypeName { get; set; }
        public IList<TypeOfGameResponseForGameType> TypeOfGames { get; set; }
    }
}