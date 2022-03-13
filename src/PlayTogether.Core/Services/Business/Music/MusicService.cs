using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Music
{
    public class MusicService : IMusicService
    {
        // private readonly IMusicRepository _musicRepository;
        // private readonly ILogger<MusicService> _logger;

        // public MusicService(
        //     IMusicRepository musicRepository, 
        //     ILogger<MusicService> logger)
        // {
        //     _musicRepository = musicRepository;
        //     _logger = logger;
        // }
        // public async Task<MusicGetByIdResponse> CreateMusicAsync(MusicCreateRequest request)
        // {
        //     try {
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _musicRepository.CreateMusicAsync(request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call CreateMusicAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> DeleteMusicAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _musicRepository.DeleteMusicAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call DeleteMusicAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<PagedResult<MusicGetByIdResponse>> GetAllMusicsAsync(MusicParameter param)
        // {
        //     try {
        //         return await _musicRepository.GetAllMusicsAsync(param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllMusicsAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<MusicGetByIdResponse> GetMusicByIdAsync(string id)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         return await _musicRepository.GetMusicByIdAsync(id);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetMusicByIdAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }

        // public async Task<bool> UpdateMusicAsync(string id, MusicUpdateRequest request)
        // {
        //     try {
        //         if (String.IsNullOrEmpty(id)) {
        //             throw new ArgumentNullException(nameof(id));
        //         }
        //         if (request is null) {
        //             throw new ArgumentNullException(nameof(request));
        //         }
        //         return await _musicRepository.UpdateMusicAsync(id, request);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call UpdateMusicAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }
    }
}