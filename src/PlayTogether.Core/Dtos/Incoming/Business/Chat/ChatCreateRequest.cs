using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Chat
{
    public class ChatCreateRequest
    {
        [Required]
        [MaxLength(500)]
        public string Message { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
        public bool IsActive = true;
    }
}