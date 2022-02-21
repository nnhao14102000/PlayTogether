using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfPlayer;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.GameOfPlayer
{
    public class GameOfPlayerService : IGameOfPlayerService
    {
        private readonly IGameOfPlayerRepository _gameOfPlayerRepository;
        private readonly ILogger<GameOfPlayerService> _logger;

        public GameOfPlayerService(
            IGameOfPlayerRepository gameOfPlayerRepository,
            ILogger<GameOfPlayerService> logger)
        {
            _gameOfPlayerRepository = gameOfPlayerRepository;
            _logger = logger;
        }

        public async Task<GameOfPlayerGetByIdResponse> CreateGameOfPlayerAsync(string playerId, GameOfPlayerCreateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(playerId)) {
                    throw new ArgumentNullException(nameof(playerId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameOfPlayerRepository.CreateGameOfPlayerAsync(playerId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateGameOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteGameOfPlayerAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _gameOfPlayerRepository.DeleteGameOfPlayerAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteGameOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<IEnumerable<GamesInPlayerGetAllResponse>> GetAllGameOfPlayerAsync(string playerId)
        {
            try {
                if (String.IsNullOrEmpty(playerId)) {
                    throw new ArgumentNullException(nameof(playerId));
                }
                return await _gameOfPlayerRepository.GetAllGameOfPlayerAsync(playerId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllGameOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<GameOfPlayerGetByIdResponse> GetGameOfPlayerByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _gameOfPlayerRepository.GetGameOfPlayerByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetGameOfPlayerByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateGameOfPlayerAsync(string id, GameOfPlayerUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameOfPlayerRepository.UpdateGameOfPlayerAsync(id, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateGameOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}