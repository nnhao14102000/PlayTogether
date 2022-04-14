using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.UnActiveBalance;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.UnActiveBalance
{
    public class UnActiveBalanceService : IUnActiveBalanceService
    {
        private readonly IUnActiveBalanceRepository _unActiveBalanceRepository;
        private readonly ILogger<UnActiveBalanceService> _logger;

        public UnActiveBalanceService(IUnActiveBalanceRepository unActiveBalanceRepository, ILogger<UnActiveBalanceService> logger)
        {
            _unActiveBalanceRepository = unActiveBalanceRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> ActiveMoneyAsync(ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _unActiveBalanceRepository.ActiveMoneyAsync(principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ActiveMoneyAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<UnActiveBalanceResponse>> GetAllUnActiveBalancesAsync(ClaimsPrincipal principal, UnActiveBalanceParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _unActiveBalanceRepository.GetAllUnActiveBalancesAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllUnActiveBalancesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}