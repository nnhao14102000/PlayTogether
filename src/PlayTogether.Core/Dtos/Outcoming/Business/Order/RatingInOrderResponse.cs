using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class RatingInOrderResponse
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public float Rate { get; set; }
        public bool IsViolate { get; set; }
        public bool IsActive { get; set; }
        public bool? IsApprove { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}