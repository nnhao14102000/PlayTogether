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
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task<Result<GameCreateResponse>> CreateGameAsync(GameCreateRequest request)
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

        public async Task<Result<bool>> DeleteGameAsync(string gameId)
        {
            try {
                if (String.IsNullOrEmpty(gameId)) {
                    throw new ArgumentNullException(nameof(gameId));
                }
                return await _gameRepository.DeleteGameAsync(gameId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<GameGetAllResponse>> GetAllGamesAsync(GameParameters param)
        {
            try {
                return await _gameRepository.GetAllGamesAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllGamesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<GameGetByIdResponse>> GetGameByIdAsync(string gameId)
        {
            try {
                if (String.IsNullOrEmpty(gameId)) {
                    throw new ArgumentNullException(nameof(gameId));
                }
                return await _gameRepository.GetGameByIdAsync(gameId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetGameByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> UpdateGameAsync(string gameId, GameUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(gameId)) {
                    throw new ArgumentNullException(nameof(gameId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameRepository.UpdateGameAsync(gameId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}