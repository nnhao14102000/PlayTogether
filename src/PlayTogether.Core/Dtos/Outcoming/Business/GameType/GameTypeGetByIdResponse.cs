using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.GameType
{
    public class GameTypeGetByIdResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string OtherName { get; set; }

        public string Description {get; set; }
        public IList<TypeOfGameResponseForGameType> TypeOfGames { get; set; }
    }
}