using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.AppUser
{
    public class UserInfoForIsPlayerUpdateRequest
    {
        [Required]
        [Range(1, 5)]
        public int MaxHourHire { get; set; }

        [Required]
        [Range(10000, 1000000000000)]
        public float PricePerHour { get; set; }

        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}