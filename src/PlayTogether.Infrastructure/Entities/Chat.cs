namespace PlayTogether.Infrastructure.Entities
{
    public class Chat : BaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiveId { get; set; }
        public string Message { get; set; }
    }
}