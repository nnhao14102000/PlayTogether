using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Report
{
    public class ReportInDetailResponse
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public OrderUserResponse User { get; set; }
        public string ToUserId { get; set; }
        public OrderUserResponse ToUser { get; set; }
        public string ReportMessage { get; set; }
        public bool? IsApprove { get; set; }
        public OrderGetDetailResponse Order { get; set; }
    }
}