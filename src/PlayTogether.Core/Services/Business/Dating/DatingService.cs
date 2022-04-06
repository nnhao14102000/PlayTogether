using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
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
        public async Task<bool> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request)
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

        public async Task<bool> DeleteDatingAsync(ClaimsPrincipal principal, string datingId)
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
    }
}