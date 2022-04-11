using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.GameOfUser
{
    public class GameOfUserService : IGameOfUserService
    {
        private readonly IGameOfUserRepository _gameOfUserRepository;
        private readonly ILogger<IGameOfUserRepository> _logger;

        public GameOfUserService(
            IGameOfUserRepository gameOfUserRepository,
            ILogger<IGameOfUserRepository> logger)
        {
            _gameOfUserRepository = gameOfUserRepository;
            _logger = logger;
        }

        public async Task<Result<GameOfUserGetByIdResponse>> CreateGameOfUserAsync(
            ClaimsPrincipal principal,
            GameOfUserCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameOfUserRepository.CreateGameOfUserAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateGameOfUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(gameOfUserId)) {
                    throw new ArgumentNullException(nameof(gameOfUserId));
                }
                return await _gameOfUserRepository.DeleteGameOfUserAsync(principal, gameOfUserId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteGameOfUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<GamesOfUserResponse>> GetAllGameOfUserAsync(string userId, GameOfUserParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _gameOfUserRepository.GetAllGameOfUserAsync(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllGameOfUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<GameOfUserGetByIdResponse>> GetGameOfUserByIdAsync(string gameOfUserId)
        {
            try {
                if (String.IsNullOrEmpty(gameOfUserId)) {
                    throw new ArgumentNullException(nameof(gameOfUserId));
                }
                return await _gameOfUserRepository.GetGameOfUserByIdAsync(gameOfUserId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetGameOfUserByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> UpdateGameOfUserAsync(
            ClaimsPrincipal principal,
            string gameOfUserId,
            GameOfUserUpdateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(gameOfUserId)) {
                    throw new ArgumentNullException(nameof(gameOfUserId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _gameOfUserRepository.UpdateGameOfUserAsync(principal, gameOfUserId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateGameOfUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}