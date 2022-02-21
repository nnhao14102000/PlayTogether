using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Rank
{
    public class RankService : IRankService
    {
        private readonly IRankRepository _rankRepository;
        private readonly ILogger<RankService> _logger;

        public RankService(
            IRankRepository rankRepository,
            ILogger<RankService> logger)
        {
            _rankRepository = rankRepository;
            _logger = logger;
        }

        public async Task<RankCreateResponse> CreateRankAsync(string gameId, RankCreateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(gameId)) {
                    throw new ArgumentNullException(nameof(gameId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _rankRepository.CreateRankAsync(gameId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateRankAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteRankAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _rankRepository.DeleteRankAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteRankAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<IEnumerable<RankGetByIdResponse>> GetAllRanksInGameAsync(string gameId)
        {
            try {
                if (String.IsNullOrEmpty(gameId)) {
                    throw new ArgumentNullException(nameof(gameId));
                }
                return await _rankRepository.GetAllRanksInGameAsync(gameId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllRanksInGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<RankGetByIdResponse> GetRankByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _rankRepository.GetRankByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetRankByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateRankAsync(string id, RankUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _rankRepository.UpdateRankAsync(id, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateRankAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}