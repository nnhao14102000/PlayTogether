using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback
{
    public class UpdateFeedbackRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        [Required]
        [MaxLength(100)]
        public string TypeOfFeedback { get; set; }
    }
}