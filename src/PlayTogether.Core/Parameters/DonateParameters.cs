using System;

namespace PlayTogether.Core.Parameters
{
    public class DonateParameters : QueryStringParameters
    {
        // public bool? IsCalculateNumberOfDonateInDate { get; set; }
        public bool? IsNew { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}