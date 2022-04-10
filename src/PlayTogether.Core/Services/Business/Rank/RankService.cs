using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
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

        public async Task<Result<RankCreateResponse>> CreateRankAsync(string gameId, RankCreateRequest request)
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

        public async Task<Result<bool>> DeleteRankAsync(string id)
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

        public async Task<PagedResult<RankGetAllResponse>> GetAllRanksInGameAsync(string gameId, RankParameters param)
        {
            try {
                if (String.IsNullOrEmpty(gameId)) {
                    throw new ArgumentNullException(nameof(gameId));
                }
                return await _rankRepository.GetAllRanksInGameAsync(gameId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllRanksInGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<RankGetByIdResponse>> GetRankByIdAsync(string id)
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

        public async Task<Result<bool>> UpdateRankAsync(string id, RankUpdateRequest request)
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