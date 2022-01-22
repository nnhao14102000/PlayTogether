using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Admin;
using PlayTogether.Core.Interfaces.Services.Business.Admin;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IAdminRepository adminRepository, ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param)
        {
            try {
                return await _adminRepository.GetAllAdminsAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllAdminsAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
