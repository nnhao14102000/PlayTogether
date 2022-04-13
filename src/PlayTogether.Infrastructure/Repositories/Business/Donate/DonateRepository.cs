using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using PlayTogether.Core.Dtos.Incoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Donate;

namespace PlayTogether.Infrastructure.Repositories.Business.Donate
{
    public class DonateRepository : BaseRepository, IDonateRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DonateRepository(IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateDonateAsync(ClaimsPrincipal principal, string charityId, DonateCreateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null || user.IsActive is false || user.Status is not UserStatusConstants.Online) {
                return false;
            }
            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            if (user.UserBalance.ActiveBalance < request.Money) {
                return false;
            }

            var charity = await _context.Charities.FindAsync(charityId);
            if (charity is null || charity.IsActive is false) {
                return false;
            }

            var model = _mapper.Map<Entities.Donate>(request);
            model.UserId = user.Id;
            model.CharityId = charityId;
            model.UpdateDate = null;

            _context.Donates.Add(model);
            if (await _context.SaveChangesAsync() >= 0) {
                user.UserBalance.ActiveBalance -= request.Money;
                user.UserBalance.Balance -= request.Money;

                await _context.TransactionHistories.AddAsync(
                    Helpers.TransactionHelpers.PopulateTransactionHistory(
                        user.UserBalance.Id,
                        TransactionTypeConstants.Sub,
                        request.Money,
                        TransactionTypeConstants.Donate,
                        model.Id
                    )
                );

                await _context.Notifications.AddRangeAsync(
                    Helpers.NotificationHelpers.PopulateNotification(model.UserId,
                    $"{user.Name}, cảm ơn bạn đã donate!",
                    $"Bạn đã donate {request.Money}đ tới tổ chức {charity.OrganizationName}. Xin chân thành cảm ơn bạn!.",
                    $"{ValueConstants.BaseUrl}/v1/donates/{model.Id}"),

                    Helpers.NotificationHelpers.PopulateNotification(model.CharityId,
                    $"{user.Name}, đã donate!",
                    (String.IsNullOrEmpty(request.Message) || (String.IsNullOrWhiteSpace(request.Message)))
                    ? $"{user.Name} đã donate {request.Money}đ tới tổ chức {charity.OrganizationName} của bạn. Xin kiểm tra lại tài khoản!."
                    : $"{user.Name} đã donate {request.Money}đ tới tổ chức {charity.OrganizationName} của bạn với lời nhắn: {request.Message}. Xin kiểm tra lại tài khoản!.",
                    $"{ValueConstants.BaseUrl}/v1/donates/{model.Id}")

                );
            }

            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<DonateResponse>> GetAllDonatesAsync(ClaimsPrincipal principal, DonateParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id;

            var isCharity = false;
            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                if (charity is null) {
                    return null;
                }
                else {
                    isCharity = true;
                }
            }
            var donates = new List<Entities.Donate>();
            if (isCharity) {
                donates = await _context.Donates.Where(x => x.CharityId == charity.Id)
                                                .ToListAsync();
            }
            else {
                donates = await _context.Donates.Where(x => x.UserId == user.Id)
                                                .ToListAsync();
            }
            foreach (var donate in donates) {
                await _context.Entry(donate).Reference(x => x.User).LoadAsync();
                await _context.Entry(donate).Reference(x => x.Charity).LoadAsync();
            }

            var query = donates.AsQueryable();

            FilterDateRange(ref query, param.FromDate, param.ToDate);
            OrderNewestDonate(ref query, param.IsNew);

            donates = query.ToList();
            var response = _mapper.Map<List<DonateResponse>>(donates);
            return PagedResult<DonateResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void OrderNewestDonate(ref IQueryable<Entities.Donate> query, bool? isNew)
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

        private void FilterDateRange(ref IQueryable<Entities.Donate> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        public async Task<DonateResponse> GetDonateByIdAsync(string donateId)
        {
            var donate = await _context.Donates.FindAsync(donateId);
            if (donate is null) {
                return null;
            }
            await _context.Entry(donate).Reference(x => x.User).LoadAsync();
            await _context.Entry(donate).Reference(x => x.Charity).LoadAsync();
            return _mapper.Map<DonateResponse>(donate);
        }

        public async Task<(int, float, int, float)> CalculateDonateAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return (0, 0, 0, 0);
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (charity is null) {
                return (0, 0, 0, 0);
            }

            var donate = await _context.Donates.Where(x => x.CharityId == charity.Id).ToListAsync();
            int countNumberOfDonateInDay = CountNumberOfDonateInDay(donate);
            float totalMoneyDonatedInDay = TotalMoneyDonateReceiveInDay(donate);
            int count3 = donate.Count();
            float count4 = TotalMoneyReceive(donate);

            return (countNumberOfDonateInDay, totalMoneyDonatedInDay, count3, count4);
        }

        private float TotalMoneyReceive(List<Entities.Donate> donates)
        {
            if (!donates.Any()) {
                return 0;
            }
            float total = 0;
            foreach (var item in donates) {
                total += item.Money;
            }

            return total;
        }

        private int CountNumberOfDonateInDay(List<Entities.Donate> donates)
        {
            if (!donates.Any()) {
                return 0;
            }
            string strDate = DateTime.UtcNow.AddHours(7).ToShortDateString();
            int count = 0;
            foreach (var item in donates) {
                var date = item.CreatedDate?.ToShortDateString();
                if (date.Contains(strDate)) {
                    count++;
                }
            }
            return count;
        }

        private float TotalMoneyDonateReceiveInDay(List<Entities.Donate> donates)
        {
            if (!donates.Any()) {
                return 0;
            }
            float total = 0;
            string strDate = DateTime.UtcNow.AddHours(7).ToShortDateString();
            foreach (var item in donates) {
                var date = item.CreatedDate?.ToShortDateString();
                if (date.Contains(strDate)) {
                    total += item.Money;
                }
            }

            return total;
        }
    }
}