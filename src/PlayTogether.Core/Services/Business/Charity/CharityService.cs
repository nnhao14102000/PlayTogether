using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Interfaces.Repositories.Business.Charity;
using PlayTogether.Core.Interfaces.Services.Business.Charity;
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

        public async Task<IEnumerable<CharityResponse>> GetAllCharityAsync()
        {
            try {
                return await _charityRepository.GetAllCharityAsync();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllCharityAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
