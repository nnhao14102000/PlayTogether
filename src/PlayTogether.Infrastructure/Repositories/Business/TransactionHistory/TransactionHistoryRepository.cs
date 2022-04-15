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
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Incoming.Business.TransactionHistory;

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
            var result = new PagedResult<TransactionHistoryResponse>();
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
            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            var trans = await _context.TransactionHistories.Where(x => x.UserBalanceId == user.UserBalance.Id).ToListAsync();

            var query = trans.AsQueryable();
            FilterByTransactionType(ref query, param.Type);
            FilterByOperation(ref query, param.Operation);
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

        private void FilterByOperation(ref IQueryable<Core.Entities.TransactionHistory> query, string operation)
        {
            if(!query.Any() || String.IsNullOrEmpty(operation) || String.IsNullOrWhiteSpace(operation)){
                return ;
            }
            query = query.Where(x => x.Operation.ToLower().Contains(operation.ToLower()));
        }

        private void SortNewTransaction(ref IQueryable<Core.Entities.TransactionHistory> query, bool? isNew)
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

        private void FilterByDateRange(ref IQueryable<Core.Entities.TransactionHistory> query, DateTime? fromDate, DateTime? toDate)
        {
            if(!query.Any() || fromDate is null || toDate is null){
                return ;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        private void FilterByTransactionType(ref IQueryable<Core.Entities.TransactionHistory> query, string type)
        {
            if(!query.Any() || String.IsNullOrEmpty(type) || String.IsNullOrWhiteSpace(type)){
                return ;
            }
            query = query.Where(x => x.TypeOfTransaction.ToLower().Contains(type.ToLower()));
        }

        public async Task<PagedResult<TransactionHistoryResponse>> GetAllTransactionHistoriesForAdminAsync(string userId, TransactionParameters param)
        {
            var user = await _context.AppUsers.FindAsync(userId);
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

        public async Task<Result<bool>> DepositAsync(ClaimsPrincipal principal, DepositRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }
            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();
            user.UserBalance.Balance += request.Money;
            user.UserBalance.ActiveBalance += request.Money;
            await _context.TransactionHistories.AddAsync(Helpers.TransactionHelpers.PopulateTransactionHistory(
                user.UserBalance.Id, TransactionTypeConstants.Add, request.Money, TransactionTypeConstants.Deposit, request.MomoTransactionId
            ));
            await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                user.Id, "Nạp tiền thành công!", $"Bạn đã nạp {request.Money} vào tài khoản thành công lúc {DateTime.Now.AddHours(7)}.", ""
            ));
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;

        }
    }
}