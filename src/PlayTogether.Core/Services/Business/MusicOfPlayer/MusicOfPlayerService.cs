using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.MusicOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.MusicOfPlayer;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.MusicOfPlayer
{
    public class MusicOfPlayerService : IMusicOfPlayerService
    {
        private readonly IMusicOfPlayerRepository _musicOfPlayerRepository;
        private readonly ILogger<MusicOfPlayerService> _logger;

        public MusicOfPlayerService(
            IMusicOfPlayerRepository musicOfPlayerRepository,
            ILogger<MusicOfPlayerService> logger)
        {
            _musicOfPlayerRepository = musicOfPlayerRepository;                
            _logger = logger;                
        }
        public async Task<MusicOfPlayerGetByIdResponse> CreateMusicOfPlayerAsync(
            string playerId,
            MusicOfPlayerCreateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(playerId)) {
                    throw new ArgumentNullException(nameof(playerId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _musicOfPlayerRepository.CreateMusicOfPlayerAsync(playerId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateMusicOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteMusicOfPlayerAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _musicOfPlayerRepository.DeleteMusicOfPlayerAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteMusicOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<IEnumerable<MusicOfPlayerGetAllResponse>> GetALlMusicOfPlayerAsync(string playerId)
        {
            try {
                if (String.IsNullOrEmpty(playerId)) {
                    throw new ArgumentNullException(nameof(playerId));
                }
                return await _musicOfPlayerRepository.GetALlMusicOfPlayerAsync(playerId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetALlMusicOfPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<MusicOfPlayerGetByIdResponse> GetMusicOfPlayerByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _musicOfPlayerRepository.GetMusicOfPlayerByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetMusicOfPlayerByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}