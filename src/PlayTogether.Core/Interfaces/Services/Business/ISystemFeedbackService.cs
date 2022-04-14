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
        Task<Result<bool>> CreateFeedbackAsync(ClaimsPrincipal principal, CreateFeedbackRequest request);
        Task<Result<bool>> DeleteFeedbackAsync(ClaimsPrincipal principal, string feedbackId);
        Task<Result<bool>> UpdateFeedbackAsync(ClaimsPrincipal principal, string feedbackId, UpdateFeedbackRequest request);
        Task<Result<SystemFeedbackDetailResponse>> GetFeedbackByIdAsync(string feedbackId);
        Task<PagedResult<SystemFeedbackResponse>> GetAllFeedbacksAsync(ClaimsPrincipal principal, SystemFeedbackParameters param);
        Task<Result<bool>> ProcessFeedbackAsync(string feedbackId ,ProcessFeedbackRequest request);
    }
}