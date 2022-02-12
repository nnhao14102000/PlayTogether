using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;

namespace PlayTogether.Core.Dtos.Outcoming.Business.GameOfPlayer
{
    public class GamesInPlayerGetAllResponse
    {
        public string Id { get; set; }

        public string GameId { get; set; }
        public GameGetAllResponse Game { get; set; }

        public string Rank { get; set; }
    }
}