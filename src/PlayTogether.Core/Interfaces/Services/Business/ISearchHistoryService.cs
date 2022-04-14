using PlayTogether.Core.Dtos.Outcoming.Business.SearchHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface ISearchHistoryService
    {
        Task<PagedResult<SearchHistoryResponse>> GetAllSearchHistoryAsync(ClaimsPrincipal principal, SearchHistoryParameters param);
        Task<Result<bool>> DeleteSearchHistoryAsync(ClaimsPrincipal principal, string searchHistoryId);
    }
}