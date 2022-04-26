using System;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class GamesOfOrderCreateRequest
    {
        public string GameId { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}