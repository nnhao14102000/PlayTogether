using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.SearchHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Incoming.Generic;

namespace PlayTogether.Infrastructure.Repositories.Business.SearchHistory
{
    public class SearchHistoryRepository : BaseRepository, ISearchHistoryRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public SearchHistoryRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> DeleteSearchHistoryAsync(ClaimsPrincipal principal, string searchHistoryId)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var searchHistory = await _context.SearchHistories.FindAsync(searchHistoryId);
            if (searchHistory is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound);
                return result;
            }
            if (searchHistory.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            searchHistory.UpdateDate = DateTime.UtcNow.AddHours(7);
            searchHistory.IsActive = false;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<SearchHistoryResponse>> GetAllSearchHistoryAsync(
            ClaimsPrincipal principal,
            SearchHistoryParameters param)
        {
            var result = new PagedResult<SearchHistoryResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var searchHistories = await _context.SearchHistories.Where(x => x.UserId == user.Id).ToListAsync();
            var query = searchHistories.AsQueryable();
            FilterActiveHistory(ref query, true);
            GetHotSearch(ref query, param.IsHotSearch);
            FilterByContent(ref query, param.Content);
            SortNewSearch(ref query, param.IsNew);
            searchHistories = query.ToList();
            var response = _mapper.Map<List<SearchHistoryResponse>>(searchHistories);
            return PagedResult<SearchHistoryResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void GetHotSearch(ref IQueryable<Core.Entities.SearchHistory> query, bool? isHotSearch)
        {
            if (isHotSearch is null || isHotSearch is false) {
                return;
            }
            var listSearch = _context.SearchHistories.GroupBy(x => x.SearchString)
                                .Select(g => new { searchName = g.Key, count = g.Count() })
                                .OrderByDescending(x => x.count).ToList();

            var list = new List<Core.Entities.SearchHistory>();
            foreach (var item in listSearch) {
                var s = _context.SearchHistories.FirstOrDefault(x => x.SearchString == item.searchName);
                if (s is null) {
                    continue;
                }
                list.Add(s);
            }
            query = list.AsQueryable();
        }

        private void SortNewSearch(ref IQueryable<Core.Entities.SearchHistory> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.UpdateDate);
            }
            else {
                query = query.OrderBy(x => x.UpdateDate);
            }
        }

        private void FilterByContent(ref IQueryable<Core.Entities.SearchHistory> query, string content)
        {
            if (!query.Any() || String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content)) {
                return;
            }
            query = query.Where(x => x.SearchString.ToLower().Contains(content.ToLower()));
        }

        private void FilterActiveHistory(ref IQueryable<Core.Entities.SearchHistory> query, bool v)
        {
            if (!query.Any()) {
                return;
            }
            query = query.Where(x => x.IsActive == v);
        }
    }
}