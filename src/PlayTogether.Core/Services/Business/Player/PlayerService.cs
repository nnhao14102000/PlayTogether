using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Player;
using PlayTogether.Core.Interfaces.Services.Business.Player;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Player
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository PlayerRepository, ILogger<PlayerService> logger)
        {
            _playerRepository = PlayerRepository ?? throw new ArgumentNullException(nameof(PlayerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(PlayerParameters param)
        {
            try {
                return await _playerRepository.GetAllPlayersForHirerAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdAsync(string id)
        {
            try {
                return await _playerRepository.GetPlayerByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetPlayerByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PlayerProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        {
            try {
                return await _playerRepository.GetPlayerProfileByIdentityIdAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetPlayerProfileByIdentityIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdatePlayerInformationAsync(string id, PlayerUpdateInfoRequest request)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _playerRepository.UpdatePlayerInformationAsync(id, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdatePlayerInformationAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
