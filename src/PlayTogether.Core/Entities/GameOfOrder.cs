namespace PlayTogether.Core.Entities
{
    public class GameOfOrder : BaseEntity
    {
        public string OrderId { get; set; }
        public Order Order { get; set; }

        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}
