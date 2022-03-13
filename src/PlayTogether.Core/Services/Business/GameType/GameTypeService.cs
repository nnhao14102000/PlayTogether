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
        // private readonly IGameTypeRepository _gameTypeRepository;
        // private readonly ILogger<GameTypeService> _logger;

        // public GameTypeService(
        //     IGameTypeRepository gameTypeRepository, 
        //     ILogger<GameTypeService> logger)
        // {
        //     _gameTypeRepository = gameTypeRepository;
        //     _logger = logger;
        // }

        // public async Task<GameTypeCreateResponse> CreateGameTypeAsync(GameTypeCreateRequest request)
        // {
        //     try {
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _gameTypeRepository.CreateGameTypeAsync(request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call CreateGameTypeAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> DeleteGameTypeAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _gameTypeRepository.DeleteGameTypeAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call DeleteGameTypeAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param)
        // {
        //     try {
        //         return await _gameTypeRepository.GetAllGameTypesAsync(param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllGameTypesAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<GameTypeGetByIdResponse> GetGameTypeByIdAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _gameTypeRepository.GetGameTypeByIdAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetGameTypeByIdAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> UpdateGameTypeAsync(string id, GameTypeUpdateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _gameTypeRepository.UpdateGameTypeAsync(id, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call UpdateGameTypeAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }
    }
}