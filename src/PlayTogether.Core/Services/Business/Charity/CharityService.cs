using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
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
            _charityRepository = charityRepository;
            _logger = logger;
        }

        public async Task<bool> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request)
        {
            try {
                if(request is null){
                    throw new ArgumentNullException(nameof(request));
                }
                return await _charityRepository.ChangeStatusCharityByAdminAsync(charityId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ChangeStatusCharityByAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
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

        public async Task<CharityResponse> GetCharityByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _charityRepository.GetCharityByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetCharityByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
