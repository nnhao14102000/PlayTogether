using System.Security.Claims;
using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IRatingRepository
    {
        Task<Result<bool>> CreateRatingFeedbackAsync(ClaimsPrincipal principal, string orderId, RatingCreateRequest request);
        Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string userId, RatingParameters param);
        Task<Result<RatingGetResponse>> GetRatingByIdAsync(string ratingId);
        Task<PagedResult<RatingGetResponse>> GetAllViolateRatingsForAdminAsync(RatingParametersAdmin param);
        Task<Result<RatingGetDetailResponse>> GetRatingByDetailAdminAsync(string ratingId);
        Task<Result<bool>> ViolateRatingAsync(ClaimsPrincipal principal, string ratingId, RatingViolateRequest request);
        Task<Result<bool>> ProcessViolateRatingAsync (string ratingId, ProcessViolateRatingRequest request);
    }
}