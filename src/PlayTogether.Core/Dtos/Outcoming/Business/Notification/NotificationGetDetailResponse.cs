using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Notification
{
    public class NotificationGetDetailResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
}