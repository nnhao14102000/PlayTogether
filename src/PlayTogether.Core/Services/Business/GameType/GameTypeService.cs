using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.GameType
{
    public class GameTypeService : IGameTypeService
    {
        private readonly IGameTypeRepository _gameTypeRepository;
        private readonly ILogger<GameTypeService> _logger;

        public GameTypeService(
            IGameTypeRepository gameTypeRepository, 
            ILogger<GameTypeService> logger)
        {
            _gameTypeRepository = gameTypeRepository;
            _logger = logger;
        }

        public async Task<GameTypeCreateResponse> CreateGameTypeAsync(GameTypeCreateRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameTypeRepository.CreateGameTypeAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateGameTypeAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteGameTypeAsync(string gameTypeId)
        {
            try {
                if (String.IsNullOrEmpty(gameTypeId)) {
                    throw new ArgumentNullException(nameof(gameTypeId));
                }
                return await _gameTypeRepository.DeleteGameTypeAsync(gameTypeId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteGameTypeAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param)
        {
            try {
                return await _gameTypeRepository.GetAllGameTypesAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllGameTypesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<GameTypeGetByIdResponse> GetGameTypeByIdAsync(string gameTypeId)
        {
            try {
                if (String.IsNullOrEmpty(gameTypeId)) {
                    throw new ArgumentNullException(nameof(gameTypeId));
                }
                return await _gameTypeRepository.GetGameTypeByIdAsync(gameTypeId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetGameTypeByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateGameTypeAsync(string gameTypeId, GameTypeUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(gameTypeId)) {
                    throw new ArgumentNullException(nameof(gameTypeId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameTypeRepository.UpdateGameTypeAsync(gameTypeId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateGameTypeAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}