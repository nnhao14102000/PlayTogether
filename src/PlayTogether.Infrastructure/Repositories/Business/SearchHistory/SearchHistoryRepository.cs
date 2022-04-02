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

        public async Task<bool> DeleteSearchHistoryAsync(ClaimsPrincipal principal, string searchHistoryId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            var searchHistory = await _context.SearchHistories.FindAsync(searchHistoryId);
            if (searchHistory is null) return false;
            if (searchHistory.UserId != user.Id) return false;

            searchHistory.UpdateDate = DateTime.UtcNow.AddHours(7);
            searchHistory.IsActive = false;
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<SearchHistoryResponse>> GetAllSearchHistoryAsync(
            ClaimsPrincipal principal,
            SearchHistoryParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return null;
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

        private void GetHotSearch(ref IQueryable<Entities.SearchHistory> query, bool? isHotSearch)
        {
            if (!query.Any() || isHotSearch is null || isHotSearch is false) {
                return;
            }
            var listSearch = _context.SearchHistories.GroupBy(x => x.SearchString).Select(g => new { searchId = g.Key, count = g.Count() }).OrderByDescending(x => x.count);
            var list = new List<Entities.SearchHistory>();
            foreach (var item in listSearch) {
                var s = _context.SearchHistories.Find(item.searchId);
                if (s is null) {
                    continue;
                }
                list.Add(s);
            }
            query = list.AsQueryable();
        }

        private void SortNewSearch(ref IQueryable<Entities.SearchHistory> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.UpdateDate).ThenByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.UpdateDate).ThenBy(x => x.CreatedDate);
            }
        }

        private void FilterByContent(ref IQueryable<Entities.SearchHistory> query, string content)
        {
            if (!query.Any() || String.IsNullOrEmpty(content) || String.IsNullOrWhiteSpace(content)) {
                return;
            }
            query = query.Where(x => x.SearchString.ToLower().Contains(content.ToLower()));
        }

        private void FilterActiveHistory(ref IQueryable<Entities.SearchHistory> query, bool v)
        {
            if (!query.Any()) {
                return;
            }
            query = query.Where(x => x.IsActive == v);
        }
    }
}