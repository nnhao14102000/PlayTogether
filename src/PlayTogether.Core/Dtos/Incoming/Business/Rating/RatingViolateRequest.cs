using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Rating
{
    public class RatingViolateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Reason { get; set; }
    }
}