namespace PlayTogether.Infrastructure.Entities
{
    public class MusicOfPlayer : BaseEntity
    {
        public string MusicId { get; set; }
        public Music Music { get; set; }

        public string PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
