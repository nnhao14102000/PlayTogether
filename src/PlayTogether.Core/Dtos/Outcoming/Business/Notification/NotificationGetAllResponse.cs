using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Notification
{
    public class NotificationGetAllResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}