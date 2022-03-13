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
        // private readonly IRatingRepository _ratingRepository;
        // private readonly ILogger<RatingService> _logger;

        // public RatingService(IRatingRepository ratingRepository, ILogger<RatingService> logger)
        // {
        //     _logger = logger;
        //     _ratingRepository = ratingRepository;
        // }

        // public async Task<bool> CreateRatingFeedbackAsync(string orderId, RatingCreateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(orderId)) {
        //             throw new ArgumentNullException(nameof(orderId));
        //         }
        //         if(request is null){
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _ratingRepository.CreateRatingFeedbackAsync(orderId, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call CreateRatingFeedbackAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> DisableFeedbackAsync(string ratingId)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(ratingId)) {
        //             throw new ArgumentNullException(nameof(ratingId));
        //         }
        //         return await _ratingRepository.DisableFeedbackAsync(ratingId);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call DisableFeedbackAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PagedResult<RatingGetResponse>> GetAllRatingsAsync(string playerId, RatingParameters param)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(playerId)) {
        //             throw new ArgumentNullException(nameof(playerId));
        //         }
        //         return await _ratingRepository.GetAllRatingsAsync(playerId, param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllRatingsAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PagedResult<RatingGetResponse>> GetAllViolateRatingsForAdminAsync(RatingParametersAdmin param)
        // {
        //     try {
        //         return await _ratingRepository.GetAllViolateRatingsForAdminAsync(param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllViolateRatingsForAdminAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> ViolateFeedbackAsync(string ratingId)
        // { 
        //     try {
        //         if (String.IsNullOrEmpty(ratingId)) {
        //             throw new ArgumentNullException(nameof(ratingId));
        //         }
        //         return await _ratingRepository.ViolateFeedbackAsync(ratingId);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call ViolateFeedbackAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }
    }
}