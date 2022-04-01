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

        private void FilterByIsRelease(ref IQueryable<Entities.UnActiveBalance> query, bool? isRelease)
        {
            if (!query.Any() || isRelease is null) {
                return;
            }

            query = query.Where(x => x.IsRelease == isRelease);

        }

        private void SortNew(ref IQueryable<Entities.UnActiveBalance> query, bool? isNew)
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

        private void FilterByDateRange(ref IQueryable<Entities.UnActiveBalance> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
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

            var unActive = await _context.UnActiveBalances.Where(x => x.UserBalanceId == user.UserBalance.Id
                                                                && x.IsRelease == false).ToListAsync();

            if (unActive.Count == 0) {
                return false;
            }

            foreach (var item in unActive) {
                await _context.Entry(item).Reference(x => x.Order).Query().Include(x => x.Reports).LoadAsync();
                if (item.Order.Reports.Count > 0) {
                    foreach (var report in item.Order.Reports) {
                        if (report.IsApprove == true && report.UserId == item.Order.UserId) {
                            // + tiền lại cho user
                            var fromUser = await _context.AppUsers.FindAsync(item.Order.UserId);
                            await _context.Entry(fromUser).Reference(x => x.UserBalance).LoadAsync();
                            fromUser.UserBalance.Balance += item.Order.TotalPrices;
                            fromUser.UserBalance.ActiveBalance += item.Order.TotalPrices;
                            if (await _context.SaveChangesAsync() < 0) return false;

                            var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                            await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                            toUser.UserBalance.Balance -= item.Order.TotalPrices;
                            if (await _context.SaveChangesAsync() < 0) return false;

                            item.IsRelease = true;
                            item.UpdateDate = DateTime.UtcNow.AddHours(7);
                            await _context.TransactionHistories.AddRangeAsync(
                                Helpers.TransactionHelpers.PopulateTransactionHistory(fromUser.UserBalance.Id, "+", item.Money, "Order", item.OrderId),
                                Helpers.TransactionHelpers.PopulateTransactionHistory(toUser.UserBalance.Id, "-", item.Money, "Order", item.OrderId)
                            );
                            if (await _context.SaveChangesAsync() < 0) return false;
                        }
                        else if (report.IsApprove == false && report.UserId == item.Order.UserId) {
                            // tăng tiền cho thằng toUser như thường
                            var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                            await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                            toUser.UserBalance.ActiveBalance += item.Order.TotalPrices;
                            if (await _context.SaveChangesAsync() < 0) return false;

                            item.IsRelease = true;
                            item.UpdateDate = DateTime.UtcNow.AddHours(7);
                            if (await _context.SaveChangesAsync() < 0) return false;
                        }
                        else if (report.IsApprove == null && report.UserId == item.Order.UserId && DateTime.UtcNow.AddHours(7) >= item.DateActive) {
                            // admin chưa duyệt mà giờ cũng lỗ rồi thì công tiền luôn...
                            var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                            await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                            toUser.UserBalance.ActiveBalance += item.Order.TotalPrices;
                            if (await _context.SaveChangesAsync() < 0) return false;

                            item.IsRelease = true;
                            item.UpdateDate = DateTime.UtcNow.AddHours(7);
                            if (await _context.SaveChangesAsync() < 0) return false;
                        }
                    }
                }
                else {
                    //ko có report nào thì chỉ canh tới giờ thì cộng vào thôi
                    if (DateTime.UtcNow.AddHours(7) >= item.DateActive) {
                        // admin chưa duyệt mà giờ cũng lỗ rồi thì công tiền luôn...
                        var toUser = await _context.AppUsers.FindAsync(item.Order.ToUserId);
                        await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();
                        toUser.UserBalance.ActiveBalance += item.Order.TotalPrices;
                        if (await _context.SaveChangesAsync() < 0) return false;

                        item.IsRelease = true;
                        item.UpdateDate = DateTime.UtcNow.AddHours(7);
                        if (await _context.SaveChangesAsync() < 0) return false;
                    }
                }
            }

            return true;
        }
    }
}