using PlayTogether.Core.Dtos.Outcoming.Business.Charity;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class DonateInOrderResponse
    {
        public string CharityId { get; set; }
        public CharityResponse Charity { get; set; }
    }
}