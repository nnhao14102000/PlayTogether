using PlayTogether.Core.Dtos.Incoming.Business.SystemConfig;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Entities;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface ISystemConfigService
    {
        Task<Result<bool>> CreateConfigAsync(ConfigCreateRequest request);
        Task<Result<bool>> UpdateConfigAsync(string configId, ConfigUpdateRequest request);
        Task<Result<bool>> DeleteConfigAsync(string configId);
        Task<Result<SystemConfig>> GetSystemConfigByIdAsync(string configId);
        Task<Result<SystemConfig>> GetSystemConfigByNOAsync (int numberOfOrder);
        Task<PagedResult<SystemConfig>> GetAllSystemConfigAsync(SystemConfigParameters param);
    }
}