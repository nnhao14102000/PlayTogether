using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using System;
namespace PlayTogether.Core.Dtos.Outcoming.Business.Chat
{
    public class ChatGetResponse
    {
        public string Id { get; set; }
        public OrderUserResponse User { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}