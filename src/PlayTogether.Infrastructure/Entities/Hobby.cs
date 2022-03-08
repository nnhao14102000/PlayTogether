namespace PlayTogether.Infrastructure.Entities
{
    public class Hobby : BaseEntity
    {
        public string HirerId { get; set; }
        public Hirer Hirer { get; set; }

        public string GameId { get; set; }
        public Game Game { get; set; }
    }
}