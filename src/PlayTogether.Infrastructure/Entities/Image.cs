namespace PlayTogether.Infrastructure.Entities
{
    public class Image : BaseEntity
    {
        public string ImageLink { get; set; }
        public string PlayerId { get; set; }
        public Player Player { get; set; }
    }
}