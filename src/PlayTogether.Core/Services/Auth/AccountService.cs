﻿using Microsoft.Extensions.Logging;
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

        public async Task<AuthResult> LoginHirerByGoogleAsync(GoogleLoginRequest loginEmailDto)
        {
            try {
                if (loginEmailDto is null) {
                    throw new ArgumentNullException(nameof(loginEmailDto));
                }
                return await _accountRepository.LoginHirerByGoogleAsync(loginEmailDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginHirerByGoogle in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LoginPlayerByGoogleAsync(GoogleLoginRequest loginEmailDto)
        {
            try {
                if (loginEmailDto is null) {
                    throw new ArgumentNullException(nameof(loginEmailDto));
                }
                return await _accountRepository.LoginPlayerByGoogleAsync(loginEmailDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginPlayerByGoogle in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> LoginUserAsync(LoginRequest loginDto)
        {
            try {
                if (loginDto is null) {
                    throw new ArgumentNullException(nameof(loginDto));
                }
                return await _accountRepository.LoginUserAsync(loginDto);
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

        public async Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _accountRepository.RegisterAdminAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> RegisterCharityAsync(RegisterCharityInfoRequest registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _accountRepository.RegisterCharityAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterCharityAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> RegisterHirerAsync(RegisterUserInfoRequest registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _accountRepository.RegisterHirerAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> RegisterMultiHirerAsync(List<RegisterUserInfoRequest> registerDtos)
        {
            try {
                if (registerDtos is null) {
                    throw new ArgumentNullException(nameof(registerDtos));
                }
                return await _accountRepository.RegisterMultiHirerAsync(registerDtos);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterMultiHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> RegisterMultiPlayerAsync(List<RegisterUserInfoRequest> registerDtos)
        {
            try {
                if (registerDtos is null) {
                    throw new ArgumentNullException(nameof(registerDtos));
                }
                return await _accountRepository.RegisterMultiPlayerAsync(registerDtos);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterMultiPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResult> RegisterPlayerAsync(RegisterUserInfoRequest registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _accountRepository.RegisterPlayerAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterPlayerAsync in service class, Error Message: {ex}.");
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