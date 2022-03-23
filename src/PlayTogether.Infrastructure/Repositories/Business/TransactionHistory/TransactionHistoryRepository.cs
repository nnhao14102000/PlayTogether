using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.TransactionHistory
{
    public class TransactionHistoryRepository : BaseRepository, ITransactionHistoryRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public TransactionHistoryRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<TransactionHistoryResponse>> GetAllTransactionHistoriesAsync(ClaimsPrincipal principal, TransactionParameters param)
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

            var trans = await _context.TransactionHistories.Where(x => x.UserBalanceId == user.UserBalance.Id).ToListAsync();

            var query = trans.AsQueryable();
            FilterByTransactionType(ref query, param.Type);
            FilterByDateRange(ref query, param.FromDate, param.ToDate);
            SortNewTransaction(ref query, param.IsNew);


            trans = query.ToList();
            var response = _mapper.Map<List<TransactionHistoryResponse>>(trans);
            return PagedResult<TransactionHistoryResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void SortNewTransaction(ref IQueryable<Entities.TransactionHistory> query, bool? isNew)
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

        private void FilterByDateRange(ref IQueryable<Entities.TransactionHistory> query, DateTime? fromDate, DateTime? toDate)
        {
            if(!query.Any() || fromDate is null || toDate is null){
                return ;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        private void FilterByTransactionType(ref IQueryable<Entities.TransactionHistory> query, string type)
        {
            if(!query.Any() || String.IsNullOrEmpty(type) || String.IsNullOrWhiteSpace(type)){
                return ;
            }
            query = query.Where(x => x.TypeOfTransaction.ToLower().Contains(type.ToLower()));
        }
    }
}