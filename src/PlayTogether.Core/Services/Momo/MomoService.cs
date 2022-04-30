using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Momo;
using PlayTogether.Core.Interfaces.Services.Momo;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IMomoRepository _momoRepository;
        private readonly ILogger<MomoService> _logger;

        public MomoService(IMomoRepository momoRepository, ILogger<MomoService> logger)
        {
            _momoRepository = momoRepository;
            _logger = logger;
        }
        public async Task<Result<string>> GenerateMomoLinkAsync()
        {
            try {
                return await _momoRepository.GenerateMomoLinkAsync();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GenerateMomoLinkAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}