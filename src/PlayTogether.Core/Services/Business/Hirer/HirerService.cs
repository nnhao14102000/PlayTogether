using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
using PlayTogether.Core.Parameters;
using System;
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

        public async Task<PagedResult<HirerResponse>> GetAllHirersAsync(HirerParameters param)
        {
            try {
                return await _hirerRepository.GetAllHirersAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllHirersAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
