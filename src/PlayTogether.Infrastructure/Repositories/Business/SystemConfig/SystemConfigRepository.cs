using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.SystemConfig;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.SystemConfig
{
    public class SystemConfigRepository : BaseRepository, ISystemConfigRepository
    {
        public SystemConfigRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<bool>> CreateConfigAsync(ConfigCreateRequest request)
        {
            var result = new Result<bool>();
            var exitConfig = await _context.SystemConfigs.AnyAsync(x => (x.Title).ToLower() == request.Title.ToLower());
            if (exitConfig is true) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" cấu hình có tên {request.Title}");
                return result;
            }

            var model = _mapper.Map<Core.Entities.SystemConfig>(request);
            await _context.SystemConfigs.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteConfigAsync(string configId)
        {
            var result = new Result<bool>();
            var config = await _context.SystemConfigs.FindAsync(configId);
            if (config is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thông tin cấu hình mà bạn muốn xóa. Vui lòng thử lại.");
                return result;
            }

            _context.SystemConfigs.Remove(config);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<Core.Entities.SystemConfig>> GetAllSystemConfigAsync(SystemConfigParameters param)
        {
            var configs = await _context.SystemConfigs.OrderBy(x => x.NO).ToListAsync();
            var query = configs.AsQueryable();

            FilterByName(ref query, param.Title);

            configs = query.ToList();
            return PagedResult<Core.Entities.SystemConfig>.ToPagedList(configs, param.PageNumber, param.PageSize);
        }

        private void FilterByName(ref IQueryable<Core.Entities.SystemConfig> query, string title)
        {
            if(!query.Any() || String.IsNullOrEmpty(title) || String.IsNullOrWhiteSpace(title)){
                return;
            }
            query = query.Where(x => x.Title.ToLower().Contains(title.ToLower()));
        }

        public async Task<Result<Core.Entities.SystemConfig>> GetSystemConfigByIdAsync(string configId)
        {
            var result = new Result<Core.Entities.SystemConfig>();
            var config = await _context.SystemConfigs.FindAsync(configId);

            if (config is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" cấu hình.");
                return result;
            }
            result.Content = config;
            return result;
        }

        public async Task<Result<bool>> UpdateConfigAsync(string configId, ConfigUpdateRequest request)
        {
            var result = new Result<bool>();
            var config = await _context.SystemConfigs.FindAsync(configId);

            if (config is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" cấu hình.");
                return result;
            }

            _context.SystemConfigs.Update(config);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}