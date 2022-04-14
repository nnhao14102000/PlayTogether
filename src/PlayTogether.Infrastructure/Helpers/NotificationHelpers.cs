using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Entities;
using System;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class NotificationHelpers
    {
        public static Notification PopulateNotification(string receiveId, string title, string message, string link)
        {
            return new Notification {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.UtcNow.AddHours(7),
                UpdateDate = null,
                ReceiverId = receiveId,
                Title = title,
                Message = message,
                ReferenceLink = link,
                Status = NotificationStatusConstants.NotRead
            };
        }
    }
}