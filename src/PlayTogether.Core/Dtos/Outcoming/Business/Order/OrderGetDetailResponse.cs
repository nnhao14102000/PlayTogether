using System.Collections.Generic;
using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class OrderGetDetailResponse
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public OrderUserResponse User { get; set; }

        public string ToUserId { get; set; }
        public OrderUserResponse ToUser { get; set; }

        public IList<GameOfOrderResponse> GameOfOrders { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public int TotalTimes { get; set; }
        public float TotalPrices { get; set; }
        public float FinalPrices { get; set; }

        public ICollection<RatingInOrderResponse> Ratings { get; set; }
        public ICollection<ReportInOrderResponse> Reports { get; set; }
        
        public DateTime TimeStart { get; set; }
        public DateTime TimeFinish { get; set; }
        public string Status { get; set; }
    }
}