using PlayTogether.Core.Dtos.Outcoming.Business.UnActiveBalance;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IUnActiveBalanceService
    {
        Task<PagedResult<UnActiveBalanceResponse>> GetAllUnActiveBalancesAsync(ClaimsPrincipal principal, UnActiveBalanceParameters param);
        Task<Result<bool>> ActiveMoneyAsync(ClaimsPrincipal principal);
    }
}