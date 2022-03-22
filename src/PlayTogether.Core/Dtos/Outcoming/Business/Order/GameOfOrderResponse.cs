namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class GameOfOrderResponse
    {
        public string Id { get; set; }
        public string GameId { get; set; }
        public GameResponse Game { get; set; }
    }
}