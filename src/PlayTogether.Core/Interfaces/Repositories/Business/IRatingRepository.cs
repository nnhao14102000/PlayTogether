using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IRatingRepository
    {
        Task<bool> CreateRatingFeedbackAsync(string orderId, RatingCreateRequest request);
        Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string userId, RatingParameters param);
        Task<PagedResult<RatingGetResponse>> GetAllViolateRatingsForAdminAsync(RatingParametersAdmin param);
        Task<bool> ViolateRatingAsync(string ratingId);
        Task<bool> ProcessViolateRatingAsync (string ratingId, ProcessViolateRatingRequest request);
    }
}