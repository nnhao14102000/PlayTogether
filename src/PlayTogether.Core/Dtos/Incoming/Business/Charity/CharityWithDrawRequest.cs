using System;
namespace PlayTogether.Core.Dtos.Incoming.Business.Charity
{
    public class CharityWithDrawRequest
    {
        public float MoneyWithdraw { get; set; }
        public bool IsSuccess = true;
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}