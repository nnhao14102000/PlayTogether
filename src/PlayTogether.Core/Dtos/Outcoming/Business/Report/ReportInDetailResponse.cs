using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Report
{
    public class ReportInDetailResponse
    {
        public string Id { get; set; }
        public OrderGetDetailResponse Order { get; set; }
    }
}