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

            searchHistory.UpdateDate = DateTime.Now;
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
            FilterByContent(ref query, param.Content);
            SortDscByCreateDate(ref query, param.SortDsc);
            searchHistories = query.ToList();
            var response = _mapper.Map<List<SearchHistoryResponse>>(searchHistories);
            return PagedResult<SearchHistoryResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void SortDscByCreateDate(ref IQueryable<Entities.SearchHistory> query, bool? sortDsc)
        {
            if (!query.Any() || sortDsc is null) {
                return;
            }
            if (sortDsc is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
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