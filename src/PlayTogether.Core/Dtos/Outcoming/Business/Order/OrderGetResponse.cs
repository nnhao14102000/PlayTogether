using System;
using System.Collections.Generic;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class OrderGetResponse
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
        public float PercentSub {get; set;}

        public string Status { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeFinish { get; set; }
        public DateTime ProcessExpired {get; set; }
    }
}