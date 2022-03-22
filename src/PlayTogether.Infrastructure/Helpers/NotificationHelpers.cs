using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Infrastructure.Entities;
using System;

namespace PlayTogether.Infrastructure.Helpers
{
    public static class NotificationHelpers
    {
        public static Notification PopulateNotification(string receiveId, string title, string message, string link)
        {
            return new Notification {
                Id = Guid.NewGuid().ToString(),
                CreatedDate = DateTime.Now,
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