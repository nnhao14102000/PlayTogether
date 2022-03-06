using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Order
{
    public class ReportInOrderResponse
    {
        public string Id { get; set; }
        public string ReportMessage { get; set; }
        public bool IsApprove { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}