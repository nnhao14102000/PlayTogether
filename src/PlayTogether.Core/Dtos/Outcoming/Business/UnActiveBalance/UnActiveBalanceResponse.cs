using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.UnActiveBalance
{
    public class UnActiveBalanceResponse
    {
        public string Id { get; set; }
        public float Money { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}