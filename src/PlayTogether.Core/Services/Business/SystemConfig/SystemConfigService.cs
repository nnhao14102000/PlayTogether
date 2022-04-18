using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.SystemConfig;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.SystemConfig
{
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ISystemConfigRepository _systemConfigRepository;
        private readonly ILogger<SystemConfigService> _logger;

        public SystemConfigService(ISystemConfigRepository systemConfigRepository, ILogger<SystemConfigService> logger)
        {
            _systemConfigRepository = systemConfigRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> CreateConfigAsync(ConfigCreateRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _systemConfigRepository.CreateConfigAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateConfigAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteConfigAsync(string configId)
        {
            try {
                if (String.IsNullOrEmpty(configId)) {
                    throw new ArgumentNullException(nameof(configId));
                }
                return await _systemConfigRepository.DeleteConfigAsync(configId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteConfigAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<Entities.SystemConfig>> GetAllSystemConfigAsync(SystemConfigParameters param)
        {
            try {
                return await _systemConfigRepository.GetAllSystemConfigAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllSystemConfigAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<Entities.SystemConfig>> GetSystemConfigByIdAsync(string configId)
        {
            try {
                if (String.IsNullOrEmpty(configId)) {
                    throw new ArgumentNullException(nameof(configId));
                }
                return await _systemConfigRepository.GetSystemConfigByIdAsync(configId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetSystemConfigByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> UpdateConfigAsync(string configId, ConfigUpdateRequest request)
        {
            try {
                if (String.IsNullOrEmpty(configId)) {
                    throw new ArgumentNullException(nameof(configId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _systemConfigRepository.UpdateConfigAsync(configId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateConfigAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}