using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.TypeOfGame
{
    public class TypeOfGameService : ITypeOfGameService
    {
        private readonly ITypeOfGameRepository _typeOfGameRepository;
        private readonly ILogger<TypeOfGameService> _logger;

        public TypeOfGameService(
            ITypeOfGameRepository typeOfGameRepository, 
            ILogger<TypeOfGameService> logger)
        {
            _typeOfGameRepository = typeOfGameRepository;
            _logger = logger;
        }

        public async Task<bool> CreateTypeOfGameAsync(
            TypeOfGameCreateRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _typeOfGameRepository.CreateTypeOfGameAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateTypeOfGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteTypeOfGameAsync(string typeOfGameId)
        {
            try {
                if (String.IsNullOrEmpty(typeOfGameId)) {
                    throw new ArgumentNullException(nameof(typeOfGameId));
                }
                return await _typeOfGameRepository.DeleteTypeOfGameAsync(typeOfGameId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteTypeOfGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<TypeOfGameGetByIdResponse> GetTypeOfGameByIdAsync(string typeOfGameId)
        {
            try {
                if (String.IsNullOrEmpty(typeOfGameId)) {
                    throw new ArgumentNullException(nameof(typeOfGameId));
                }
                return await _typeOfGameRepository.GetTypeOfGameByIdAsync(typeOfGameId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetTypeOfGameByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}