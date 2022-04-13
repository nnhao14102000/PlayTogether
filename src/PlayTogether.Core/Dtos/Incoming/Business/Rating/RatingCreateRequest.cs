using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Rating
{
    public class RatingCreateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Comment { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rate { get; set; }
        public bool IsViolate = false;
        public bool IsActive = true;
        public bool? IsApprove = null;
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}