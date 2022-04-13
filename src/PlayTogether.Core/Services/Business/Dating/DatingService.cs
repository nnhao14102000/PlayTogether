using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Dating
{
    public class DatingService : IDatingService
    {
        private readonly IDatingRepository _datingRepository;
        private readonly ILogger<DatingService> _logger;

        public DatingService(IDatingRepository datingRepository, ILogger<DatingService> logger)
        {
            _datingRepository = datingRepository;
            _logger = logger;
        }
        public async Task<Result<bool>> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _datingRepository.CreateDatingAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateDatingAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteDatingAsync(ClaimsPrincipal principal, string datingId)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(datingId) || String.IsNullOrWhiteSpace(datingId)) {
                    throw new ArgumentNullException(nameof(datingId));
                }
                return await _datingRepository.DeleteDatingAsync(principal, datingId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteDatingAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<DatingUserResponse>> GetAllDatingsOfUserAsync(string userId, DatingParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _datingRepository.GetAllDatingsOfUserAsync(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllDatingsOfUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<DatingUserResponse>> GetDatingByIdAsync(string datingId)
        {
            try {
                if (String.IsNullOrEmpty(datingId) || String.IsNullOrWhiteSpace(datingId)) {
                    throw new ArgumentNullException(nameof(datingId));
                }
                return await _datingRepository.GetDatingByIdAsync(datingId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetDatingByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> UpdateDatingAsync(ClaimsPrincipal principal, string datingId, DatingUpdateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                if (String.IsNullOrEmpty(datingId) || String.IsNullOrWhiteSpace(datingId)) {
                    throw new ArgumentNullException(nameof(datingId));
                }
                return await _datingRepository.UpdateDatingAsync(principal, datingId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateDatingAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}