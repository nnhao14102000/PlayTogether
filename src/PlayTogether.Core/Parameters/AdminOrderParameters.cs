using System;

namespace PlayTogether.Core.Parameters
{
    public class AdminOrderParameters: QueryStringParameters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate {get;set;}
        public string Status { get; set; }
    }
}