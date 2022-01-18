using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Interfaces.Repositories.Business.Admin;
using PlayTogether.Core.Interfaces.Services.Business.Admin;
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

        public async Task<IEnumerable<AdminResponse>> GetAllAdminAsync()
        {
            try {
                return await _adminRepository.GetAllAdminAsync();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
