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

        public async Task<Result<bool>> CreateDonateAsync(ClaimsPrincipal principal, string charityId, DonateCreateRequest request)
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
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hi???n t??i kho???n b???n ??ang ??? ch??? ????? {user.Status}. Vui l??ng th??? l???i sau khi t???t c??? c??c thao t??c ho??n t???t.");
                return result;
            }
            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            if (user.UserBalance.ActiveBalance < request.Money) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"T??i kho???n kh??? d???ng c???a b???n ({user.UserBalance.ActiveBalance}) hi???n ??ang kh??ng ????? ????? donate.");
                return result;
            }

            var charity = await _context.Charities.FindAsync(charityId);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y t??? ch???c t??? thi???n n??y.");
                return result;
            }

            if (charity.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "T??? ch???c t??? thi???n n??y hi???n kh??ng kh??? d???ng.");
                return result;
            }

            var model = _mapper.Map<Core.Entities.Donate>(request);
            model.UserId = user.Id;
            model.CharityId = charityId;
            model.UpdateDate = null;

            _context.Donates.Add(model);
            if (await _context.SaveChangesAsync() >= 0) {
                user.UserBalance.ActiveBalance -= request.Money;
                user.UserBalance.Balance -= request.Money;
                charity.Balance += request.Money;

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
                    $"{user.Name}, c???m ??n b???n ???? donate!",
                    $"B???n ???? donate {request.Money}?? t???i t??? ch???c {charity.OrganizationName}. Xin ch??n th??nh c???m ??n b???n!.",
                    $"{ValueConstants.BaseUrl}/v1/donates/{model.Id}"),

                    Helpers.NotificationHelpers.PopulateNotification(model.CharityId,
                    $"{user.Name}, ???? donate!",
                    (String.IsNullOrEmpty(request.Message) || (String.IsNullOrWhiteSpace(request.Message)))
                    ? $"{user.Name} ???? donate {request.Money}?? t???i t??? ch???c {charity.OrganizationName} c???a b???n. Xin ki???m tra l???i t??i kho???n!."
                    : $"{user.Name} ???? donate {request.Money}?? t???i t??? ch???c {charity.OrganizationName} c???a b???n v???i l???i nh???n: {request.Message}. Xin ki???m tra l???i t??i kho???n!.",
                    $"{ValueConstants.BaseUrl}/v1/donates/{model.Id}")

                );
                if (await _context.SaveChangesAsync() >= 0) {
                    result.Content = true;
                    return result;
                }
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<DonateResponse>> GetAllDonatesAsync(ClaimsPrincipal principal, DonateParameters param)
        {
            var result = new PagedResult<DonateResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var isCharity = false;
            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                if (charity is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "KH??ng x??c ?????nh ???????c t??i kho???n.");
                    return result;
                }
                else {
                    isCharity = true;
                }
            }
            var donates = new List<Core.Entities.Donate>();
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

        private void OrderNewestDonate(ref IQueryable<Core.Entities.Donate> query, bool? isNew)
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

        private void FilterDateRange(ref IQueryable<Core.Entities.Donate> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        public async Task<Result<DonateResponse>> GetDonateByIdAsync(string donateId)
        {
            var result = new Result<DonateResponse>();
            var donate = await _context.Donates.FindAsync(donateId);
            if (donate is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" th??ng tin donate.");
                return result;
            }
            await _context.Entry(donate).Reference(x => x.User).LoadAsync();
            await _context.Entry(donate).Reference(x => x.Charity).LoadAsync();
            var response = _mapper.Map<DonateResponse>(donate);
            result.Content = response;
            return result;
        }

        public async Task<Result<(int, float, int, float)>> CalculateDonateAsync(ClaimsPrincipal principal)
        {
            var result = new Result<(int, float, int, float)>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.Unauthenticate);
                return result;
            }

            var donate = await _context.Donates.Where(x => x.CharityId == charity.Id).ToListAsync();
            int countNumberOfDonateInDay = CountNumberOfDonateInDay(donate);
            float totalMoneyDonatedInDay = TotalMoneyDonateReceiveInDay(donate);
            int count3 = donate.Count();
            float count4 = TotalMoneyReceive(donate);
            result.Content = (countNumberOfDonateInDay, totalMoneyDonatedInDay, count3, count4);
            return result;
        }

        private float TotalMoneyReceive(List<Core.Entities.Donate> donates)
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

        private int CountNumberOfDonateInDay(List<Core.Entities.Donate> donates)
        {
            if (!donates.Any()) {
                return 0;
            }
            string strDate = DateTime.UtcNow.AddHours(7).ToShortDateString();
            int count = 0;
            foreach (var item in donates) {
                var date = item.CreatedDate.ToShortDateString();
                if (date.Contains(strDate)) {
                    count++;
                }
            }
            return count;
        }

        private float TotalMoneyDonateReceiveInDay(List<Core.Entities.Donate> donates)
        {
            if (!donates.Any()) {
                return 0;
            }
            float total = 0;
            string strDate = DateTime.UtcNow.AddHours(7).ToShortDateString();
            foreach (var item in donates) {
                var date = item.CreatedDate.ToShortDateString();
                if (date.Contains(strDate)) {
                    total += item.Money;
                }
            }

            return total;
        }
    }
}