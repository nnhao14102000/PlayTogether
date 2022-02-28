using Microsoft.Extensions.Logging;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Donate
{
    public class DonateService : IDonateService
    {
        private readonly IDonateRepository _donateRepository;
        private readonly ILogger<DonateService> _logger;

        public DonateService(IDonateRepository donateRepository, ILogger<DonateService> logger)
        {
            _donateRepository = donateRepository;
            _logger = logger;
        }

        public async Task<int> CalculateDonateAsync(ClaimsPrincipal principal, DonateParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                
                return await _donateRepository.CalculateDonateAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CalculateTurnDonateInDayAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}