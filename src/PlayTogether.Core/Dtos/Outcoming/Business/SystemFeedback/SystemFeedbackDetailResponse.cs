using PlayTogether.Core.Dtos.Outcoming.Business.Order;

namespace PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback
{
    public class SystemFeedbackDetailResponse
    {
        public string UserId { get; set; }
        public OrderUserResponse User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string TypeOfFeedback { get; set; }
        public bool? IsApprove { get; set; }
    }
}