using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
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

        public async Task<bool> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request)
        {
            try {
                if(String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)){
                    throw new ArgumentNullException(nameof(userId));
                }
                if (request is null){
                    throw new ArgumentNullException(nameof(request));
                }
                return await _appUserRepository.ChangeIsActiveUserForAdminAsync(userId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ChangeIsActiveUserForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> ChangeIsPlayerAsync(ClaimsPrincipal principal, UserIsPlayerChangeRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null){
                    throw new ArgumentNullException(nameof(request));
                }
                return await _appUserRepository.ChangeIsPlayerAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ChangeIsPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<UserSearchResponse>> GetAllUsersAsync(ClaimsPrincipal principal, UserParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _appUserRepository.GetAllUsersAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllUsersAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param)
        {
            try {
                return await _appUserRepository.GetAllUsersForAdminAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllUsersForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
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

        public async Task<UserGetBasicInfoResponse> GetUserBasicInfoByIdAsync(string userId)
        {
            try {
                if(String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)){
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _appUserRepository.GetUserBasicInfoByIdAsync(userId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetUserBasicInfoByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<UserGetServiceInfoResponse> GetUserServiceInfoByIdAsync(string userId)
        {
            try {
                if(String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)){
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _appUserRepository.GetUserServiceInfoByIdAsync(userId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetUserServiceInfoByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdatePersonalInfoAsync(
            ClaimsPrincipal principal,
            UserPersonalInfoUpdateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null){
                    throw new ArgumentNullException(nameof(request));
                }
                return await _appUserRepository.UpdatePersonalInfoAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdatePersonalInfoAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateUserServiceInfoAsync(
            ClaimsPrincipal principal,
            UserInfoForIsPlayerUpdateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null){
                    throw new ArgumentNullException(nameof(request));
                }
                return await _appUserRepository.UpdateUserServiceInfoAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateUserServiceInfoAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}