namespace PlayTogether.Infrastructure.Entities
{
    public class TypeOfGame : BaseEntity
    {
        public GameType GameType { get; set; }
        public string GameTypeId { get; set; }

        public Game Game { get; set; }
        public string GameId { get; set; }
    }
}
