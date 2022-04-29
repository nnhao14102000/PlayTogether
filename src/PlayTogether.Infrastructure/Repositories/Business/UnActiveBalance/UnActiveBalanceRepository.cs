using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
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
            var result = new PagedResult<UnActiveBalanceResponse>();
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

            var unActive = await _context.UnActiveBalances.Where(x => x.UserBalanceId == user.UserBalance.Id).ToListAsync();

            var query = unActive.AsQueryable();
            FilterByDateRange(ref query, param.FromDate, param.ToDate);
            FilterByIsRelease(ref query, param.IsRelease);
            SortNew(ref query, param.IsNew);


            unActive = query.ToList();
            var response = _mapper.Map<List<UnActiveBalanceResponse>>(unActive);
            return PagedResult<UnActiveBalanceResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterByIsRelease(ref IQueryable<Core.Entities.UnActiveBalance> query, bool? isRelease)
        {
            if (!query.Any() || isRelease is null) {
                return;
            }

            query = query.Where(x => x.IsRelease == isRelease);

        }

        private void SortNew(ref IQueryable<Core.Entities.UnActiveBalance> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterByDateRange(ref IQueryable<Core.Entities.UnActiveBalance> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        public async Task<Result<bool>> ActiveMoneyAsync(ClaimsPrincipal principal)
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

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            var unActive = await _context.UnActiveBalances.Where(x => x.UserBalanceId == user.UserBalance.Id
                                                                && x.IsRelease == false).ToListAsync();

            if (unActive.Count == 0) {
                result.Content = true;
                return result;
            }
            else {
                foreach (var item in unActive) {
                    await _context.Entry(item).Reference(x => x.Order).Query().Include(x => x.Reports).LoadAsync();

                    if (item.Order.Reports.Count > 0) { // any reports in order
                        foreach (var report in item.Order.Reports) { // get each report in reports
                            if (report.IsApprove == true && report.UserId == item.Order.UserId) {
                                // report accept, report is from user create order (Hirer)
                                var fromUser = await _context.AppUsers.FindAsync(item.Order.UserId);
                                await _context.Entry(fromUser).Reference(x => x.UserBalance).LoadAsync();
                                fromUser.UserBalance.Balance += item.Order.FinalPrices / (1 - item.Order.PercentSub);
                                fromUser.UserBalance.ActiveBalance += item.Order.FinalPrices / (1 - item.Order.PercentSub);
                                if (await _context.SaveChangesAsync() < 0) {
                                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                    return result;
                                }

                                var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                                await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                                toUser.UserBalance.Balance -= item.Order.FinalPrices;
                                if (await _context.SaveChangesAsync() < 0) {
                                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                    return result;
                                }

                                item.IsRelease = true;
                                item.UpdateDate = DateTime.UtcNow.AddHours(7);
                                await _context.TransactionHistories.AddRangeAsync(
                                    Helpers.TransactionHelpers.PopulateTransactionHistory(fromUser.UserBalance.Id, TransactionTypeConstants.Add, item.Order.FinalPrices / (1 - item.Order.PercentSub), TransactionTypeConstants.ReportRefund, item.OrderId),
                                    Helpers.TransactionHelpers.PopulateTransactionHistory(toUser.UserBalance.Id, TransactionTypeConstants.Sub, item.Order.FinalPrices, TransactionTypeConstants.Repoft, item.OrderId)
                                );

                                if (await _context.SaveChangesAsync() >= 0) {
                                    result.Content = true;
                                    return result;
                                }
                                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                return result;
                            }
                            else if (report.IsApprove == false && report.UserId == item.Order.UserId) {
                                // Report is reject, report from user create Order (Hirer)
                                var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                                await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                                toUser.UserBalance.ActiveBalance += item.Order.FinalPrices;
                                if (await _context.SaveChangesAsync() < 0) {
                                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                    return result;
                                }

                                item.IsRelease = true;
                                item.UpdateDate = DateTime.UtcNow.AddHours(7);
                                if (await _context.SaveChangesAsync() >= 0) {
                                    result.Content = true;
                                    return result;
                                }
                                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                return result;
                            }
                            else if (report.IsApprove == null && report.UserId == item.Order.UserId && DateTime.UtcNow.AddHours(7) >= item.DateActive) {
                                // report not process, report from user create Order (Hirer), out of time release
                                var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                                await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                                toUser.UserBalance.ActiveBalance += item.Order.FinalPrices;
                                if (await _context.SaveChangesAsync() < 0) {
                                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                    return result;
                                }

                                item.IsRelease = true;
                                item.UpdateDate = DateTime.UtcNow.AddHours(7);
                                if (await _context.SaveChangesAsync() >= 0) {
                                    result.Content = true;
                                    return result;
                                }
                                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                return result;
                            }
                            else if (DateTime.UtcNow.AddHours(7) >= item.DateActive) {
                                // report of toUser, report is approve or not, or not process, don't care these report, check unActive to time release, release
                                var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                                await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                                toUser.UserBalance.ActiveBalance += item.Order.FinalPrices;
                                if (await _context.SaveChangesAsync() < 0) {
                                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                    return result;
                                }

                                item.IsRelease = true;
                                item.UpdateDate = DateTime.UtcNow.AddHours(7);
                                if (await _context.SaveChangesAsync() >= 0) {
                                    result.Content = true;
                                    return result;
                                }
                                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                return result;
                            }
                        }
                    }
                    else {
                        if (DateTime.UtcNow.AddHours(7) >= item.DateActive) {
                            // Not any reports, just check to time, release it.
                            var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                            await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                            toUser.UserBalance.ActiveBalance += item.Order.FinalPrices;
                            if (await _context.SaveChangesAsync() < 0) {
                                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                                return result;
                            }

                            item.IsRelease = true;
                            item.UpdateDate = DateTime.UtcNow.AddHours(7);
                            if (await _context.SaveChangesAsync() >= 0) {
                                result.Content = true;
                                return result;
                            }
                            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                            return result;
                        }
                    }
                }
            }
            result.Content = true;
            return result;
        }
    }
}