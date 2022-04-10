using System;

namespace PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback
{
    public class SystemFeedbackResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string TypeOfFeedback { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsApprove { get; set; }
    }
}