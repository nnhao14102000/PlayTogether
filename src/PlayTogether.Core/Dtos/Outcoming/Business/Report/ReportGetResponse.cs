using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.Report
{
    public class ReportGetResponse
    {
        public string Id { get; set; }
        public string ReportMessage { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}