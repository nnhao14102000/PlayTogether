using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using PlayTogether.Core.Interfaces.Services.Auth;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository authRepository, ILogger<AuthService> logger)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> CheckExistEmailAsync(string email)
        {
            try {
                if (email is null) {
                    throw new ArgumentNullException(nameof(email));
                }
                return await _authRepository.CheckExistEmailAsync(email);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CheckExistEmailAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> LoginHirerByGoogleAsync(GoogleLoginDto loginEmailDto)
        {
            try {
                if (loginEmailDto is null) {
                    throw new ArgumentNullException(nameof(loginEmailDto));
                }
                return await _authRepository.LoginHirerByGoogleAsync(loginEmailDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginHirerByGoogle in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> LoginPlayerByGoogleAsync(GoogleLoginDto loginEmailDto)
        {
            try {
                if (loginEmailDto is null) {
                    throw new ArgumentNullException(nameof(loginEmailDto));
                }
                return await _authRepository.LoginPlayerByGoogleAsync(loginEmailDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginPlayerByGoogle in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> LoginUserAsync(LoginDto loginDto)
        {
            try {
                if (loginDto is null) {
                    throw new ArgumentNullException(nameof(loginDto));
                }
                return await _authRepository.LoginUserAsync(loginDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call LoginUserAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> RegisterAdminAsync(RegisterAdminInfoDto registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _authRepository.RegisterAdminAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> RegisterCharityAsync(RegisterCharityInfoDto registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _authRepository.RegisterCharityAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterCharityAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> RegisterHirerAsync(RegisterUserInfoDto registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _authRepository.RegisterHirerAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<AuthResultDto> RegisterPlayerAsync(RegisterUserInfoDto registerDto)
        {
            try {
                if (registerDto is null) {
                    throw new ArgumentNullException(nameof(registerDto));
                }
                return await _authRepository.RegisterPlayerAsync(registerDto);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RegisterPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}