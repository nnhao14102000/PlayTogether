using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Hobby
{
    public class HobbyService : IHobbyService
    {
        private readonly IHobbyRepository _hobbyRepository;
        private readonly ILogger<HobbyService> _logger;

        public HobbyService(IHobbyRepository hobbyRepository, ILogger<HobbyService> logger)
        {
            _hobbyRepository = hobbyRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> CreateHobbiesAsync(ClaimsPrincipal principal, List<HobbyCreateRequest> requests)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (requests is null) {
                    throw new ArgumentNullException(nameof(requests));
                }
                return await _hobbyRepository.CreateHobbiesAsync(principal, requests);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateHobbyAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<HobbiesGetAllResponse>> GetAllHobbiesAsync(string userId, HobbyParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _hobbyRepository.GetAllHobbiesAsync(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllHobbiesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteHobbyAsync(ClaimsPrincipal principal, string hobbyId)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(hobbyId)) {
                    throw new ArgumentNullException(nameof(hobbyId));
                }
                return await _hobbyRepository.DeleteHobbyAsync(principal, hobbyId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RemoveHobbyAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteRangesHobbiesAsync(ClaimsPrincipal principal, List<HobbyDeleteRequest> hobbyIds)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (hobbyIds is null || hobbyIds.Count == 0) {
                    throw new ArgumentNullException(nameof(hobbyIds));
                }
                return await _hobbyRepository.DeleteRangesHobbiesAsync(principal, hobbyIds);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteRangesHobbiesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}