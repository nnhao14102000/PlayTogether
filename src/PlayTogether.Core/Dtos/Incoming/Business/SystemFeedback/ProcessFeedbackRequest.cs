using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback
{
    public class ProcessFeedbackRequest
    {
        [Required]
        public bool IsApprove { get; set; }
    }
}