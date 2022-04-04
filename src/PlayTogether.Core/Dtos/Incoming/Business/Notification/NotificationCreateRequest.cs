using PlayTogether.Core.Dtos.Incoming.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Notification
{
    public class NotificationCreateRequest
    {
        [MaxLength(100)]
        [Required]
        public string ReceiverId { get; set; }

        [MaxLength(500)]
        [Required]
        public string Title { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Message { get; set; }
        public string ReferenceLink { get; set; }
        public string Status = NotificationStatusConstants.NotRead;
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}