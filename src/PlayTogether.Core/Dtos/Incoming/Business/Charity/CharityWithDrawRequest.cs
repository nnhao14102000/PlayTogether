using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Charity
{
    public class CharityWithDrawRequest
    {
        [Required]
        public float MoneyWithdraw { get; set; }
        public bool IsSuccess = true;
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}