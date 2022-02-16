using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class OrderGetByIdResponse
    {
        public string Id { get; set; }

        public string PlayerId { get; set; }

        public string HirerId { get; set; }

        public DateTime TimeStart { get; set; }

        public string Message { get; set; }

        public int TotalTimes { get; set; }

        public float TotalPrices { get; set; }
        
        public string Status { get; set; }
    }
}