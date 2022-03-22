using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IRatingService
    {
        Task<bool> CreateRatingFeedbackAsync(string orderId, RatingCreateRequest request);
        Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string userId, RatingParameters param);
        Task<PagedResult<RatingGetResponse>> GetAllViolateRatingsForAdminAsync(RatingParametersAdmin param);
        Task<bool> ViolateFeedbackAsync(string ratingId);
        Task<bool> DisableFeedbackAsync (string ratingId);
    }
}