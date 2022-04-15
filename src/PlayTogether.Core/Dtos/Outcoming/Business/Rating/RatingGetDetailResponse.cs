using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Rating
{
    public class RatingGetDetailResponse
    {
        public string Id { get; set; }
        public UserRateResponse User { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Comment { get; set; }
        public bool IsViolate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApprove { get; set; }
        public OrderGetDetailResponse Order { get; set; }
    }
}