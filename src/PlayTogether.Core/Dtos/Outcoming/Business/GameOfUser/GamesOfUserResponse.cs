using PlayTogether.Core.Dtos.Outcoming.Business.Game;

namespace PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser
{
    public class GamesOfUserResponse
    {
        public string Id { get; set; }

        public string GameId { get; set; }
        public GameGetAllResponse Game { get; set; }

        public string RankId { get; set; }
    }
}