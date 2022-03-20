using PlayTogether.Core.Dtos.Outcoming.Business.SearchHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ISearchHistoryRepository
    {
        Task<PagedResult<SearchHistoryResponse>> GetAllSearchHistoryAsync(ClaimsPrincipal principal, SearchHistoryParameters param);
        Task<bool> DeleteSearchHistoryAsync(ClaimsPrincipal principal, string searchHistoryId);
    }
}