using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
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

        public HirerService(IHirerRepository HirerRepository, ILogger<HirerService> logger)
        {
            _hirerRepository = HirerRepository ?? throw new ArgumentNullException(nameof(HirerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<HirerGetByIdForHirerResponse> GetHirerByIdAsync(string id)
        {
            try {
                return await _hirerRepository.GetHirerByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetHirerByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<HirerProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        {
            try {
                return await _hirerRepository.GetHirerProfileByIdentityIdAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetHirerProfileByIdentityIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateHirerInformationAsync(string id, HirerUpdateInfoRequest request)
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
                _logger.LogError($"Error while trying to call UpdateHirerProfileAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<HirerGetAllResponse>> GetAllHirersAsync(HirerParameters param)
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
