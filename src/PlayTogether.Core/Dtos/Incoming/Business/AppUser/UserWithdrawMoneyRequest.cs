using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.AppUser
{
    public class UserWithdrawMoneyRequest
    {
        [Required]
        [Range(1, float.MaxValue)]
        public float MoneyWithdraw { get; set; }
        
        [Required]
        [MaxLength(15)]
        public string PhoneNumberMomo { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}