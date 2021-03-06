using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Player
{
    public class PlayerService : IPlayerService
    {
        // private readonly IPlayerRepository _playerRepository;
        // private readonly ILogger<PlayerService> _logger;

        // public PlayerService(
        //     IPlayerRepository PlayerRepository, 
        //     ILogger<PlayerService> logger)
        // {
        //     _playerRepository = PlayerRepository;
        //     _logger = logger;
        // }

        // public async Task<bool> AcceptPolicyAsync(ClaimsPrincipal principal, PlayerAcceptPolicyRequest request)
        // {
        //     try {
        //         if (principal is null) {
        //             throw new ArgumentNullException(nameof(principal));
        //         }
                
        //         return await _playerRepository.AcceptPolicyAsync(principal, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call AcceptPolicyAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PagedResult<PlayerGetAllResponseForAdmin>> GetAllPlayersForAdminAsync(PlayerForAdminParameters param)
        // {
        //     try {
        //         return await _playerRepository.GetAllPlayersForAdminAsync(param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllPlayersForAdminAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(
        //     ClaimsPrincipal principal,
        //     PlayerParameters param)
        // {
        //     try {
        //         if (principal is null) {
        //             throw new ArgumentNullException(nameof(principal));
        //         }

        //         return await _playerRepository.GetAllPlayersForHirerAsync(principal, param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllPlayersForHirerAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PlayerGetByIdForAdminResponse> GetPlayerByIdForAdminAsync(string playerId)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(playerId)) {
        //             throw new ArgumentNullException(nameof(playerId));
        //         }
        //         return await _playerRepository.GetPlayerByIdForAdminAsync(playerId);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetPlayerByIdForAdminAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PlayerGetByIdResponseForHirer> GetPlayerByIdForHirerAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _playerRepository.GetPlayerByIdForHirerAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetPlayerByIdForHirerAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdForPlayerAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _playerRepository.GetPlayerByIdForPlayerAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetPlayerByIdForPlayerAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PlayerOtherSkillResponse> GetPlayerOtherSkillByIdAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _playerRepository.GetPlayerOtherSkillByIdAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetPlayerOtherSkillByIdAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PlayerGetProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        // {
        //     try {
        //         if (principal is null) {
        //             throw new ArgumentNullException(nameof(principal));
        //         }
        //         return await _playerRepository.GetPlayerProfileByIdentityIdAsync(principal);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetPlayerProfileByIdentityIdAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PlayerServiceInfoResponseForPlayer> GetPlayerServiceInfoByIdForPlayerAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _playerRepository.GetPlayerServiceInfoByIdForPlayerAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetPlayerServiceInfoByIdForPlayerAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> UpdatePlayerInformationAsync(string id, PlayerInfoUpdateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _playerRepository.UpdatePlayerInformationAsync(id, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call UpdatePlayerInformationAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> UpdatePlayerOtherSkillAsync(string id, OtherSkillUpdateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _playerRepository.UpdatePlayerOtherSkillAsync(id, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call UpdatePlayerOtherSkillAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> UpdatePlayerServiceInfoAsync(string id, PlayerServiceInfoUpdateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _playerRepository.UpdatePlayerServiceInfoAsync(id, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call UpdatePlayerServiceInfoAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> UpdatePlayerStatusForAdminAsync(string playerId, PlayerStatusUpdateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(playerId)) {
        //             throw new ArgumentNullException(nameof(playerId));
        //         }
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _playerRepository.UpdatePlayerStatusForAdminAsync(playerId, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call UpdatePlayerStatusForAdminAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }
    }
}
