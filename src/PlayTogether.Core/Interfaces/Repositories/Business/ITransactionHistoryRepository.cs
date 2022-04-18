using PlayTogether.Core.Dtos.Incoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ITransactionHistoryRepository
    {
        Task<PagedResult<TransactionHistoryResponse>> GetAllTransactionHistoriesAsync(ClaimsPrincipal principal, TransactionParameters param);

        Task<PagedResult<TransactionHistoryResponse>> GetAllTransactionHistoriesForAdminAsync(string userId, TransactionParameters param);
        Task<Result<bool>> DepositAsync(ClaimsPrincipal principal, DepositRequest request);
        // Task<Result<(float, float, float)>> CalculateTypeMoney(ClaimsPrincipal principal);
    }
}