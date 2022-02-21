using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Notification
{
    public class NotificationGetResponse
    {
        public string Id { get; set; }
        public string ReceiverId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}