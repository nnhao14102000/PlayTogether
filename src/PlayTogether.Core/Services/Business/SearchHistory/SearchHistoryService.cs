using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.SearchHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.SearchHistory
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly ISearchHistoryRepository _searchHistoryRepository;
        private readonly ILogger<SearchHistoryService> _logger;

        public SearchHistoryService(ISearchHistoryRepository searchHistoryRepository, ILogger<SearchHistoryService> logger)
        {
            _searchHistoryRepository = searchHistoryRepository;
            _logger = logger;
        }
        public async Task<Result<bool>> DeleteSearchHistoryAsync(ClaimsPrincipal principal, string searchHistoryId)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(searchHistoryId)) {
                    throw new ArgumentNullException(nameof(searchHistoryId));
                }
                return await _searchHistoryRepository.DeleteSearchHistoryAsync(principal, searchHistoryId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteSearchHistoryAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<SearchHistoryResponse>> GetAllSearchHistoryAsync(ClaimsPrincipal principal, SearchHistoryParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _searchHistoryRepository.GetAllSearchHistoryAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllSearchHistoryAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}