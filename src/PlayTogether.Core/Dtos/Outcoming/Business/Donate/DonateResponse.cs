using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Donate
{
    public class DonateResponse
    {
        public string UserId { get; set; }
        public OrderUserResponse User { get; set; }

        public CharityResponse Charity { get; set; }
        public string CharityId { get; set; }

        public float Money { get; set; }
    }
}