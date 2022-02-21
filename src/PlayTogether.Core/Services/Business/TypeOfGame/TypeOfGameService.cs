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

        public async Task<TypeOfGameGetByIdResponse> CreateTypeOfGameAsync(
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

        public async Task<bool> DeleteTypeOfGameAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _typeOfGameRepository.DeleteTypeOfGameAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteTypeOfGameAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<TypeOfGameGetByIdResponse> GetTypeOfGameByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _typeOfGameRepository.GetTypeOfGameByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetTypeOfGameByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}