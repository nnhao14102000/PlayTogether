using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Rating
{
    public class ProcessViolateRatingRequest
    {
        [Required]
        public bool IsApprove { get; set; }
    }
}