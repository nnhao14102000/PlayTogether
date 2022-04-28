using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Donate
{
    public class DonateCreateRequest
    {
        [Required]
        [Range(1000, float.MaxValue)]
        public float Money { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}