using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.AppUser
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly ILogger<AppUserService> _logger;

        public AppUserService(
            IAppUserRepository appUserRepository,
            ILogger<AppUserService> logger)
        {
            _appUserRepository = appUserRepository;
            _logger = logger;
        }

        public async Task<PersonalInfoResponse> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _appUserRepository.GetPersonalInfoByIdentityIdAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetPersonalInfoByIdentityIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}