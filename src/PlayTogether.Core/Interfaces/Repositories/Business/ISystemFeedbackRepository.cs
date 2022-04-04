using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ISystemFeedbackRepository
    {
        Task<bool> CreateFeedbackAsync(ClaimsPrincipal principal, CreateFeedbackRequest request);
        Task<bool> DeleteFeedbackAsync(ClaimsPrincipal principal, string feedbackId);
        Task<bool> UpdateFeedbackAsync(ClaimsPrincipal principal, string feedbackId, UpdateFeedbackRequest request);
        Task<SystemFeedbackDetailResponse> GetFeedbackByIdAsync(string feedbackId);
        Task<PagedResult<SystemFeedbackResponse>> GetAllFeedbacksAsync(ClaimsPrincipal principal, SystemFeedbackParameters param);
        Task<bool> ProcessFeedbackAsync(string feedbackId ,ProcessFeedbackRequest request);
    }
}   