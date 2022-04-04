using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface ISystemFeedbackService
    {
        Task<bool> CreateFeedbackAsync(ClaimsPrincipal principal, CreateFeedbackRequest request);
        Task<bool> DeleteFeedbackAsync(ClaimsPrincipal principal, string feedbackId);
        Task<bool> UpdateFeedbackAsync(ClaimsPrincipal principal, string feedBackId, UpdateFeedbackRequest request);
        Task<SystemFeedbackDetailResponse> GetFeedbackByIdAsync(string feedBackId);
        Task<PagedResult<SystemFeedbackResponse>> GetAllFeedbacksAsync(ClaimsPrincipal principal, SystemFeedbackParameters param);
        Task<bool> ProcessFeedbackAsync(string feedBackId, ProcessFeedbackRequest request);
    }
}