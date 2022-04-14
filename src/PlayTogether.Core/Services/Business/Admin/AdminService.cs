using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(IAdminRepository adminRepository, ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository;
            _logger = logger;
        }

        // public async Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param)
        // {
        //     try {
        //         return await _adminRepository.GetAllAdminsAsync(param);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call GetAllAdminsAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }


        public async Task<Result<(int, int, int, int)>> AdminStatisticAsync()
        {
            try {
                return await _adminRepository.AdminStatisticAsync();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call AdminStatisticAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        
    }
}
