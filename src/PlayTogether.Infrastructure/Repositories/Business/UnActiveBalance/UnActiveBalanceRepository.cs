using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.UnActiveBalance;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.UnActiveBalance
{
    public class UnActiveBalanceRepository : BaseRepository, IUnActiveBalanceRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public UnActiveBalanceRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<UnActiveBalanceResponse>> GetAllUnActiveBalancesAsync(
            ClaimsPrincipal principal,
            UnActiveBalanceParameters param)
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
            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            var unActive = await _context.UnActiveBalances.Where(x => x.UserBalanceId == user.UserBalance.Id).ToListAsync();

            var query = unActive.AsQueryable();
            FilterByDateRange(ref query, param.FromDate, param.ToDate);
            SortNew(ref query, param.IsNew);


            unActive = query.ToList();
            var response = _mapper.Map<List<UnActiveBalanceResponse>>(unActive);
            return PagedResult<UnActiveBalanceResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void SortNew(ref IQueryable<Entities.UnActiveBalance> query, bool? isNew)
        {
            if(!query.Any() || isNew is null){
                return ;
            }
            if(isNew is true){
                query = query.OrderByDescending(x => x.CreatedDate);
            }else{
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterByDateRange(ref IQueryable<Entities.UnActiveBalance> query, DateTime? fromDate, DateTime? toDate)
        {
            if(!query.Any() || fromDate is null || toDate is null){
                return ;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        public async Task<bool> ActiveMoneyAsync(ClaimsPrincipal principal)
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

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            var unActive = await _context.UnActiveBalances.Where(x => x.UserBalanceId == user.UserBalance.Id).ToListAsync();

            foreach (var item in unActive)
            {
                await _context.Entry(item).Reference(x => x.Order).Query().Include(x => x.Reports).LoadAsync();
                
            }

            return false;
        }
    }
}