namespace PlayTogether.Infrastructure.Entities
{
    public class SystemFeedback : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Message { get; set; }
    }
}
