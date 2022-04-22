using System;

namespace PlayTogether.Core.Parameters
{
    public class CharityWithdrawParameters : QueryStringParameters
    {
        public bool? IsNew { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}