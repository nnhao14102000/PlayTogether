using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using PlayTogether.Core.Interfaces.Services.Auth;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Auth
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<AuthResult> ChangePasswordAsync(ChangePasswordRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.ChangePasswordAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ChangePasswordAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> CheckExistEmailAsync(string email)
        {
            try {
                if (String.IsNullOrEmpty(email)) {
                    throw new ArgumentNullException(nameof(email));
                }
                return await _accountRepository.CheckExistEmailAsync(email);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CheckExistEmailAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LoginUserByGoogleAsync(GoogleLoginRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.LoginUserByGoogleAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginUserByGoogleAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LoginUserAsync(LoginRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.LoginUserAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LogoutAsync(ClaimsPrincipal principal)
        {
            try {
                return await _accountRepository.LogoutAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LogoutAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.RegisterAdminAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> RegisterCharityAsync(RegisterCharityInfoRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.RegisterCharityAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterCharityAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> RegisterUserAsync(RegisterUserInfoRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.RegisterUserAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LoginCharityAsync(LoginRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.LoginCharityAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginCharityAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LoginAdminAsync(LoginRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.LoginAdminAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> RegisterMultiUserAsync(List<RegisterUserInfoRequest> request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.RegisterMultiUserAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterMultiUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> RegisterMultiUserIsPlayerAsync(List<RegisterUserInfoRequest> request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.RegisterMultiUserIsPlayerAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterMultiUserIsPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> ResetPasswordAdminAsync(ResetPasswordAdminRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.ResetPasswordAdminAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ResetPasswordAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.ResetPasswordAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ResetPasswordAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> ResetPasswordTokenAsync(ResetPasswordTokenRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _accountRepository.ResetPasswordTokenAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ResetPasswordTokenAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}