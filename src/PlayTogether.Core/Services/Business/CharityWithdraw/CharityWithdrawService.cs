using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.CharityWithdraw
{
    public class CharityWithdrawService : ICharityWithdrawService
    {
        private readonly ICharityWithdrawRepository _charityWithdrawRepository;
        private readonly ILogger<CharityWithdrawService> _logger;

        public CharityWithdrawService(ICharityWithdrawRepository charityWithdrawRepository, ILogger<CharityWithdrawService> logger)
        {
            _charityWithdrawRepository = charityWithdrawRepository;
            _logger = logger;
        }
        public async Task<PagedResult<Entities.CharityWithdraw>> GetAllCharityWithdrawHistoriesAsync(string charityId, CharityWithdrawParameters param)
        {
            try {
                if (String.IsNullOrEmpty(charityId) || String.IsNullOrWhiteSpace(charityId)) {
                    throw new ArgumentNullException(nameof(charityId));
                }
                return await _charityWithdrawRepository.GetAllCharityWithdrawHistoriesAsync(charityId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllCharityWithdrawHistoriesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}