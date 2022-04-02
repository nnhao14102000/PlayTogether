using System;

namespace PlayTogether.Core.Parameters
{
    public class TransactionParameters : QueryStringParameters
    {
        public bool? IsNew { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}