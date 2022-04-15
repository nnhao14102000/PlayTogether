using System.Security.Claims;
using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Rating
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly ILogger<RatingService> _logger;

        public RatingService(IRatingRepository ratingRepository, ILogger<RatingService> logger)
        {
            _logger = logger;
            _ratingRepository = ratingRepository;
        }

        public async Task<Result<bool>> CreateRatingFeedbackAsync(ClaimsPrincipal principal, string orderId, RatingCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _ratingRepository.CreateRatingFeedbackAsync(principal, orderId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateRatingFeedbackAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> ProcessViolateRatingAsync(string ratingId, ProcessViolateRatingRequest request)
        {
            try {
                if (String.IsNullOrEmpty(ratingId)) {
                    throw new ArgumentNullException(nameof(ratingId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _ratingRepository.ProcessViolateRatingAsync(ratingId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ProcessViolateRatingAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string userId, RatingParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _ratingRepository.GetAllRatingsAsync(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllRatingsAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<RatingGetResponse>> GetAllViolateRatingsForAdminAsync(RatingParametersAdmin param)
        {
            try {
                return await _ratingRepository.GetAllViolateRatingsForAdminAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllViolateRatingsForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> ViolateRatingAsync(string ratingId, RatingViolateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(ratingId)) {
                    throw new ArgumentNullException(nameof(ratingId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(ratingId));
                }
                return await _ratingRepository.ViolateRatingAsync(ratingId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ViolateRatingAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<RatingGetResponse>> GetRatingByIdAsync(string ratingId)
        {
            try {
                if (String.IsNullOrEmpty(ratingId)) {
                    throw new ArgumentNullException(nameof(ratingId));
                }
                return await _ratingRepository.GetRatingByIdAsync(ratingId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetRatingByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<RatingGetDetailResponse>> GetRatingByDetailAdminAsync(string ratingId)
        {
            try {
                if (String.IsNullOrEmpty(ratingId)) {
                    throw new ArgumentNullException(nameof(ratingId));
                }
                return await _ratingRepository.GetRatingByDetailAdminAsync(ratingId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetRatingByDetailAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}