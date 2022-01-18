using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Interfaces.Repositories.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Hirer
{
    public class HirerService : IHirerService
    {
        private readonly IHirerRepository _hirerRepository;
        private readonly ILogger<HirerService> _logger;

        public HirerService(IHirerRepository HirerRepository, ILogger<HirerService> logger)
        {
            _hirerRepository = HirerRepository ?? throw new ArgumentNullException(nameof(HirerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<HirerResponse>> GetAllHirerAsync()
        {
            try {
                return await _hirerRepository.GetAllHirerAsync();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
