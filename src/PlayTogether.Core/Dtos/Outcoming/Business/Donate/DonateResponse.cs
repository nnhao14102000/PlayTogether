using System;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Donate
{
    public class DonateResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public OrderUserResponse User { get; set; }

        public CharityResponse Charity { get; set; }
        public string CharityId { get; set; }

        public float Money { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}