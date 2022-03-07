using System;

namespace PlayTogether.Core.Parameters
{
    public class ReportParamters : QueryStringParameters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsNew { get; set; }
    }
}