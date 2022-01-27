using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Charity
{
    public class CharityService : ICharityService
    {
        private readonly ICharityRepository _charityRepository;
        private readonly ILogger<CharityService> _logger;

        public CharityService(ICharityRepository charityRepository, ILogger<CharityService> logger)
        {
            _charityRepository = charityRepository ?? throw new ArgumentNullException(nameof(charityRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param)
        {
            try {
                return await _charityRepository.GetAllCharitiesAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllCharitiesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
