using System.Collections.Generic;
using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class OrderGetDetailResponse
    {
        public string Id { get; set; }

        public PlayerOrderResponse Player { get; set; }

        public HirerOrderResponse Hirer { get; set; }

        public DateTime TimeStart { get; set; }

        public string Message { get; set; }

        public int TotalTimes { get; set; }

        public float TotalPrices { get; set; }

        public ICollection<RatingInOrderResponse> Ratings { get; set; }
        public ICollection<ReportInOrderResponse> Reports { get; set; }

        public string Status { get; set; }
    }
}