using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Hirer
{
    public class HirerService : IHirerService
    {
        private readonly IHirerRepository _hirerRepository;
        private readonly ILogger<HirerService> _logger;

        public HirerService(
            IHirerRepository HirerRepository, 
            ILogger<HirerService> logger)
        {
            _hirerRepository = HirerRepository;
            _logger = logger;
        }

        public async Task<HirerGetByIdResponse> GetHirerByIdForHirerAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _hirerRepository.GetHirerByIdForHirerAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetHirerByIdForHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<HirerGetProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _hirerRepository.GetHirerProfileByIdentityIdAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetHirerProfileByIdentityIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateHirerInformationAsync(string id, HirerInfoUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _hirerRepository.UpdateHirerInformationAsync(id, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateHirerInformationAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<HirerGetAllResponseForAdmin>> GetAllHirersForAdminAsync(HirerParameters param)
        {
            try {
                return await _hirerRepository.GetAllHirersForAdminAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllHirersForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
