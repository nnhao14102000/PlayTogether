namespace PlayTogether.Infrastructure.Entities
{
    public class Notification : BaseEntity
    {
        public string ReceiverId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}