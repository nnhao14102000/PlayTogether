using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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

        public async Task<Result<bool>> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _charityRepository.ChangeStatusCharityByAdminAsync(charityId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ChangeStatusCharityByAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> CharityWithDrawAsync(ClaimsPrincipal principal, CharityWithDrawRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }

                return await _charityRepository.CharityWithDrawAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CharityWithDrawAsync in service class, Error Message: {ex}.");
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

        public async Task<Result<CharityResponse>> GetCharityByIdAsync(string charityId)
        {
            try {
                if (String.IsNullOrEmpty(charityId) || String.IsNullOrWhiteSpace(charityId)) {
                    throw new ArgumentNullException(nameof(charityId));
                }
                return await _charityRepository.GetCharityByIdAsync(charityId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetCharityByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<CharityResponse>> GetProfileAsync(ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }

                return await _charityRepository.GetProfileAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetProfileAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> UpdateProfileAsync(ClaimsPrincipal principal, string charityId, CharityUpdateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                if (String.IsNullOrEmpty(charityId)) {
                    throw new ArgumentNullException(nameof(charityId));
                }
                return await _charityRepository.UpdateProfileAsync(principal, charityId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateProfileAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
