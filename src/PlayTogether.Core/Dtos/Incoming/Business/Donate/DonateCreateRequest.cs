using System;
namespace PlayTogether.Core.Dtos.Incoming.Business.Donate
{
    public class DonateCreateRequest
    {
        public float Money { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}