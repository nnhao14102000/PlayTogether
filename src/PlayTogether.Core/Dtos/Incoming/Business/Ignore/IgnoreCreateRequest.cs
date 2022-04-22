using System;

namespace PlayTogether.Core.Dtos.Incoming.Business.Ignore
{
    public class IgnoreCreateRequest
    {
        public int TimeIgnore { get; set; }
        public bool IsForever { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
        public bool IsActive = true;
    }
}