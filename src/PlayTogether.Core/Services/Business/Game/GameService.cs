using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Game
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GameService> _logger;

        public GameService(
            IGameRepository gameRepository, 
            ILogger<GameService> logger)
        {
            _gameRepository = gameRepository 
                ?? throw new ArgumentNullException(nameof(gameRepository));
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GameCreateResponse> CreateGameAsync(GameCreateRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameRepository.CreateGameAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteGameAsync(string id)
        {
            try {
                if (id is null) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _gameRepository.DeleteGameAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<GameGetAllResponse>> GetAllGamesAsync(GameParameter param)
        {
            try {
                return await _gameRepository.GetAllGamesAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllGamesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<GameGetByIdResponse> GetGameByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _gameRepository.GetGameByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetGameByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateGameAsync(string id, GameUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameRepository.UpdateGameAsync(id, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}