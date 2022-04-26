using System;

namespace PlayTogether.Core.Parameters
{
    public class SystemFeedbackParameters : QueryStringParameters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate {get;set;}
        public bool? IsNew { get; set; }
        public bool? IsApprove { get; set; }
        public bool GetAll { get; set; }
        public string Type { get; set; }
    }
}