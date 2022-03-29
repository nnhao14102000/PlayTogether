using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Hirer
{
    public class HirerStatusUpdateRequest
    {
        [Required]
        public bool IsActive { get; set; }

        [MaxLength(300)]
        public string Message { get; set; }
        public DateTime UpdateDate = DateTime.UtcNow.AddHours(7);
    }
}