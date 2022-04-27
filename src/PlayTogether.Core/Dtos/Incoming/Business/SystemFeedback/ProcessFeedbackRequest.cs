using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback
{
    public class ProcessFeedbackRequest
    {
        [Required]
        [Range(0,1)]
        public int IsApprove { get; set; }
    }
}