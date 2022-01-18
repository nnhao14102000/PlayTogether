namespace PlayTogether.Infrastructure.Entities
{
    public class RankOfPlayer : BaseEntity
    {
        public string RankId { get; set; }
        public Rank Rank { get; set; }


        public string PlayerId { get; set; }
        public Player Player { get; set; }

        public string GameOfPlayerId { get; set; }
        public GameOfPlayer GamesOfPlayer { get; set; }
    }
}
