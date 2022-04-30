using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.AppUser
{
    public class AppUserRepository : BaseRepository, IAppUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AppUserRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> ChangeIsPlayerAsync(ClaimsPrincipal principal, UserIsPlayerChangeRequest request)
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
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            if (request.IsPlayer is true) {
                if (user.PricePerHour < ValueConstants.PricePerHourMinValue) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Giá tiền thuê trong 1h của bạn hiện tại là {user.PricePerHour}. Thấp hơn mức qui định là {ValueConstants.PricePerHourMinValue}. Vui lòng chỉnh sửa thông tin và thử lại.");
                    return result;
                }

                if (user.MaxHourHire < ValueConstants.MaxHourHireMinValue
                    || user.MaxHourHire > ValueConstants.MaxHourHireMaxValue) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Số giờ thuê tối đa 1 lượt thuê của bạn nên ở trong mức từ {ValueConstants.MaxHourHireMinValue} giờ đến {ValueConstants.MaxHourHireMaxValue} giờ. Vui lòng chỉnh sửa thông tin và thử lại.");
                    return result;
                }

                var gameOfUserExist = await _context.GameOfUsers.AnyAsync(x => x.UserId == user.Id);
                if (!gameOfUserExist) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Bạn chưa thêm kĩ năng game nào vào danh sách kĩ năng. Vui lòng thêm các game mà bạn có kĩ năng tốt và thử lại.");
                    return result;
                }

            }
            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<PersonalInfoResponse>> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal)
        {
            var result = new Result<PersonalInfoResponse>();
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

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();
            await _context.Entry(user).Reference(x => x.BehaviorPoint).LoadAsync();
            await _context.Entry(user).Collection(x => x.Images).LoadAsync();
            await _context.Entry(user).Collection(x => x.Datings).Query().OrderBy(x => x.DayInWeek).ThenBy(x => x.FromHour).LoadAsync();

            var rates = await _context.Ratings.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orders = await _context.Orders.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orderOnTimes = await _context.Orders.Where(x => x.ToUserId == user.Id
                                                                && x.Status == OrderStatusConstants.Complete).ToListAsync();
            double totalTime = 0;
            foreach (var item in orders) {
                if (item.Status == OrderStatusConstants.Complete || item.Status == OrderStatusConstants.FinishSoonHirer || item.Status == OrderStatusConstants.FinishSoonPlayer) {
                    totalTime += Helpers.UtilsHelpers.GetTime(item.TimeStart, item.TimeFinish);
                }

            }
            user.NumOfRate = rates.Count();
            user.NumOfOrder = orders.Where(x => x.Status != OrderStatusConstants.Cancel && x.Status != OrderStatusConstants.OverTime && x.Status != OrderStatusConstants.Reject && x.Status != OrderStatusConstants.Interrupt).Count();
            user.TotalTimeOrder = Convert.ToInt32(Math.Ceiling(totalTime / 3600));
            user.NumOfFinishOnTime = orderOnTimes.Count();

            if (await _context.SaveChangesAsync() < 0) {
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }


            var response = _mapper.Map<PersonalInfoResponse>(user);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateUserServiceInfoAsync(ClaimsPrincipal principal, UserInfoForIsPlayerUpdateRequest request)
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
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request)
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

            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<UserGetBasicInfoResponse>> GetUserBasicInfoByIdAsync(string userId)
        {
            var result = new Result<UserGetBasicInfoResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }
            await _context.Entry(user).Collection(x => x.Images).LoadAsync();
            await _context.Entry(user).Collection(x => x.Datings).Query().OrderBy(x => x.DayInWeek).ThenBy(x => x.FromHour).LoadAsync();
            await _context.Entry(user).Reference(x => x.BehaviorPoint).LoadAsync();

            var rates = await _context.Ratings.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orders = await _context.Orders.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orderOnTimes = await _context.Orders.Where(x => x.ToUserId == user.Id
                                                                && x.Status == OrderStatusConstants.Complete).ToListAsync();
            double totalTime = 0;
            foreach (var item in orders) {
                if (item.Status == OrderStatusConstants.Complete || item.Status == OrderStatusConstants.FinishSoonHirer || item.Status == OrderStatusConstants.FinishSoonPlayer) {
                    totalTime += Helpers.UtilsHelpers.GetTime(item.TimeStart, item.TimeFinish);
                }
            }
            user.NumOfRate = rates.Count();
            user.NumOfOrder = orders.Where(x => x.Status != OrderStatusConstants.Cancel && x.Status != OrderStatusConstants.OverTime && x.Status != OrderStatusConstants.Reject && x.Status != OrderStatusConstants.Interrupt).Count();
            user.TotalTimeOrder = Convert.ToInt32(Math.Ceiling(totalTime / 3600));
            user.NumOfFinishOnTime = orderOnTimes.Count();

            if (await _context.SaveChangesAsync() < 0) {
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }

            var response = _mapper.Map<UserGetBasicInfoResponse>(user);
            result.Content = response;
            return result;
        }

        public async Task<Result<UserGetServiceInfoResponse>> GetUserServiceInfoByIdAsync(string userId)
        {
            var result = new Result<UserGetServiceInfoResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }
            var response = _mapper.Map<UserGetServiceInfoResponse>(user);
            result.Content = response;
            return result;
        }

        public async Task<PagedResult<UserSearchResponse>> GetAllUsersAsync(
            ClaimsPrincipal principal,
            UserParameters param)
        {
            var result = new PagedResult<UserSearchResponse>();
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

            if (user.Status == UserStatusConstants.Offline) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Offline);
                return result;
            }

            var users = await _context.AppUsers.ToListAsync();

            var query = users.AsQueryable();

            Search(ref query, user.Id, param.Search);

            FilterUserRecentHired(ref query, param.IsRecentOrder, user.Id);
            FilterHaveSkillSameHobby(ref query, param.IsSkillSameHobbies, user);
            FilterUserByGameId(ref query, param.GameId);

            FilterUserByDate(ref query, param.DayInWeek);
            FilterUserByHour(ref query, param.FromHour, param.ToHour);

            FilterUserByGender(ref query, param.Gender);
            FilterUserStatus(ref query, param.Status);

            FilterUserByRangePrice(ref query, param.FromPrice, param.ToPrice);
            FilterIsPlayerUser(ref query, param.IsPlayer);
            FilterActiveUser(ref query, true);            

            OrderUserByASCName(ref query, param.IsOrderByName);
            OrderUserByHighestRating(ref query, param.IsOrderByRating);
            OrderUserPricing(ref query, param.IsOrderByPricing);
            OrderUserByCreatedDate(ref query, param.IsNewAccount);
            FilterUserByItSelf(ref query, user.Id, param.IsOrderByRating);

            users = query.ToList();

            var response = _mapper.Map<List<UserSearchResponse>>(users);
            foreach (var item in response) {
                var rates = await _context.Ratings.Where(x => x.ToUserId == item.Id).ToListAsync();
                item.NumOfRate = rates.Count();
            }
            return result = PagedResult<UserSearchResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterUserByRangePrice(ref IQueryable<Core.Entities.AppUser> query, float? fromPrice, float? toPrice)
        {
            if (!query.Any() || fromPrice is null || toPrice is null || fromPrice >= toPrice) {
                return;
            }

            query = query.Where(x => x.PricePerHour >= fromPrice && x.PricePerHour <= toPrice);
        }

        private int CalculateDayInWeekToday(DayOfWeek dayOfWeek)
        {
            var result = 0;
            switch (dayOfWeek) {
                case DayOfWeek.Monday:
                    result = 2;
                    break;
                case DayOfWeek.Tuesday:
                    result = 3;
                    break;
                case DayOfWeek.Wednesday:
                    result = 4;
                    break;
                case DayOfWeek.Thursday:
                    result = 5;
                    break;
                case DayOfWeek.Friday:
                    result = 6;
                    break;
                case DayOfWeek.Saturday:
                    result = 7;
                    break;
                default:
                    result = 8;
                    break;
            }
            return result;
        }

        private void FilterUserByDate(ref IQueryable<Core.Entities.AppUser> query, int DayInWeek)
        {
            if (!query.Any()
               || DayInWeek == 0) {
                return;
            }
            var list = new List<Core.Entities.AppUser>();
            if (DayInWeek == 2) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 2);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (DayInWeek == 3) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 3);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (DayInWeek == 4) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 4);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (DayInWeek == 5) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 5);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (DayInWeek == 6) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 6);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (DayInWeek == 7) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 7);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (DayInWeek == 8) {
                var datings = _context.Datings.Where(x => x.DayInWeek == 8);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            query = list.AsQueryable();
        }

        private void FilterUserByHour(ref IQueryable<Core.Entities.AppUser> query, string fromHour, string toHour)
        {
            if (!query.Any()
               || String.IsNullOrEmpty(fromHour)
               || String.IsNullOrWhiteSpace(fromHour)
               || String.IsNullOrEmpty(toHour)
               || String.IsNullOrWhiteSpace(toHour)) {
                return;
            }

            int fH = Int32.Parse(fromHour);
            int tH = Int32.Parse(toHour);
            if (fH > tH) {
                return;
            }
            var list = new List<Core.Entities.AppUser>();
            var datings = _context.Datings.Where(x => x.FromHour >= fH && x.ToHour >= tH);
            foreach (var item in datings) {
                var user = _context.AppUsers.Find(item.UserId);
                list.Add(user);
            }
            query = list.AsQueryable();
        }

        public async Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param)
        {
            var users = await _context.AppUsers.ToListAsync();
            var query = users.AsQueryable();

            FilterActiveUser(ref query, param.IsActive);
            FilterByStatus(ref query, param.Status);
            FilterUserByNameVsEmail(ref query, param.Name);

            OrderUserByCreatedDate(ref query, param.IsNew);

            users = query.ToList();
            var response = _mapper.Map<List<UserGetByAdminResponse>>(users);
            return PagedResult<UserGetByAdminResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void OrderUserByCreatedDate(ref IQueryable<Core.Entities.AppUser> query, bool? isNewAccount)
        {
            if (!query.Any() || isNewAccount is null) {
                return;
            }
            if (isNewAccount is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterHaveSkillSameHobby(
            ref IQueryable<Core.Entities.AppUser> query,
            bool? isSameHobbies,
            Core.Entities.AppUser user)
        {
            if (!query.Any() || isSameHobbies is false || isSameHobbies is null) {
                return;
            }

            var final = new List<Core.Entities.AppUser>();
            var listGameOfUser = new List<Core.Entities.GameOfUser>();

            _context.Entry(user).Collection(x => x.Hobbies).Load();

            foreach (var hobby in user.Hobbies) {
                listGameOfUser = listGameOfUser.Union(_context.GameOfUsers.Where(x => x.GameId == hobby.GameId).ToList()).ToList();
            }

            foreach (var gameOfUser in listGameOfUser) {
                _context.Entry(gameOfUser).Reference(x => x.User).Load();
                final.Add(gameOfUser.User);
            }
            query = final.AsQueryable().Distinct();

        }

        private void FilterUserByItSelf(ref IQueryable<Core.Entities.AppUser> query, string id, bool? isOrderByRating)
        {
            if (!query.Any() || String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id)) {
                return;
            }
            if(isOrderByRating is true){
                return;
            }
            query = query.Where(x => x.Id != id);
        }

        private void OrderUserPricing(ref IQueryable<Core.Entities.AppUser> query, bool? isOrderByPricing)
        {
            if (!query.Any() || isOrderByPricing is null) {
                return;
            }
            if (isOrderByPricing is true) {
                query = query.OrderByDescending(x => x.PricePerHour);
            }
            else {
                query = query.OrderBy(x => x.PricePerHour);
            }
        }

        private void OrderUserByHighestRating(ref IQueryable<Core.Entities.AppUser> query, bool? isOrderByRating)
        {
            if (!query.Any() || isOrderByRating is null || isOrderByRating is false) {
                return;
            }

            query = query.OrderByDescending(x => x.RankingPoint).Where(x => x.NumOfRate != 0);
        }

        private void OrderUserByASCName(ref IQueryable<Core.Entities.AppUser> query, bool? isOrderByName)
        {
            if (!query.Any() || isOrderByName is null) {
                return;
            }
            if (isOrderByName == true) {
                query = query.OrderBy(x => x.Name);
            }
            else {
                query = query.OrderByDescending(x => x.Name);
            }
        }

        private void FilterUserByGender(
            ref IQueryable<Core.Entities.AppUser> query,
            bool? gender)
        {
            if (!query.Any() || gender is null) {
                return;
            }
            query = query.Where(x => x.Gender == gender);
        }

        private void FilterUserByGameId(
            ref IQueryable<Core.Entities.AppUser> query,
            string gameId)
        {
            if (!query.Any() || String.IsNullOrEmpty(gameId) || String.IsNullOrWhiteSpace(gameId)) {
                return;
            }
            if (gameId.Contains(" ")) {
                var seperatringGameId = gameId.Split(" ");
                List<Core.Entities.AppUser> result = new();
                foreach (var sprId in seperatringGameId) {
                    var gamesOfUser = _context.GameOfUsers.Where(x => x.GameId == sprId);
                    foreach (var item in gamesOfUser) {
                        result.Add(item.User);
                    }
                }
                query = result.Distinct().AsQueryable();
            }
            else if (gameId.Contains(",")) {
                var seperatringGameId = gameId.Split(",");
                List<Core.Entities.AppUser> result = new();
                foreach (var sprId in seperatringGameId) {
                    var gamesOfUser = _context.GameOfUsers.Where(x => x.GameId == sprId);
                    foreach (var item in gamesOfUser) {
                        result.Add(item.User);
                    }
                }
                query = result.Distinct().AsQueryable();
            }
            else {
                List<Core.Entities.AppUser> result = new();
                var gamesOfUser = _context.GameOfUsers.Where(x => x.GameId == gameId);
                foreach (var item in gamesOfUser) {
                    result.Add(item.User);
                }
                query = result.AsQueryable();
            }

        }

        private void FilterUserRecentHired(ref IQueryable<Core.Entities.AppUser> query, bool? isRecent, string userId)
        {
            if ((!query.Any())
                || isRecent is null
                || isRecent is false
                || String.IsNullOrEmpty(userId)
                || String.IsNullOrWhiteSpace(userId)) {
                return;
            }

            var orders = _context.Orders.Where(x => x.UserId == userId
                                                    && (x.Status == OrderStatusConstants.Complete
                                                        || x.Status == OrderStatusConstants.FinishSoonHirer
                                                        || x.Status == OrderStatusConstants.FinishSoonPlayer)
                                                        )
                                        .OrderByDescending(x => x.CreatedDate)
                                        .ToList();
            List<Core.Entities.AppUser> players = new();
            foreach (var item in orders) {
                // _context.Entry(item).Reference(x => x.User).Load();
                var player = _context.AppUsers.Find(item.ToUserId);
                players.Add(player);
            }
            query = players.AsQueryable().Distinct();
        }

        private void FilterUserByName(
            ref IQueryable<Core.Entities.AppUser> query,
            string userId,
            string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            var existSearch = _context.SearchHistories.Where(x => x.UserId == userId).Any(x => x.SearchString.ToLower() == name.ToLower());
            if (existSearch is true) {
                var search = _context.SearchHistories.FirstOrDefault(x => x.UserId == userId && x.SearchString.ToLower() == name.ToLower());
                if (search is not null) {
                    search.UpdateDate = DateTime.UtcNow.AddHours(7);
                    search.IsActive = true;
                }
            }
            else {
                _context.SearchHistories.Add(Helpers.SearchHistoryHelpers.PopulateSearchHistory(userId, name));
            }
            if (_context.SaveChanges() < 0) return;
            query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        private void FilterUserByNameVsEmail(
            ref IQueryable<Core.Entities.AppUser> query,
            string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => (x.Name.ToLower() + " " + x.Email.ToLower()).Contains(name.ToLower()));
        }

        private void FilterUserStatus(ref IQueryable<Core.Entities.AppUser> query, string userStatus)
        {
            if (!query.Any() || String.IsNullOrEmpty(userStatus) || String.IsNullOrWhiteSpace(userStatus)) {
                return;
            }
            query = query.Where(x => x.Status.ToLower() == userStatus.ToLower());
        }

        private void FilterIsPlayerUser(ref IQueryable<Core.Entities.AppUser> query, bool? isPlayer)
        {
            if (!query.Any() || isPlayer is null) {
                return;
            }
            query = query.Where(x => x.IsPlayer == isPlayer);
        }

        private void FilterActiveUser(ref IQueryable<Core.Entities.AppUser> query, bool? isActive)
        {
            if (!query.Any() || isActive is null) {
                return;
            }
            query = query.Where(x => x.IsActive == isActive);
        }

        private void Search(ref IQueryable<Core.Entities.AppUser> query, string userId, string searchString)
        {
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString)) {
                return;
            }

            var existSearch = _context.SearchHistories.Where(x => x.UserId == userId).Any(x => x.SearchString.ToLower() == searchString.ToLower());
            if (existSearch is true) {
                var search = _context.SearchHistories.FirstOrDefault(x => x.UserId == userId && x.SearchString.ToLower() == searchString.ToLower());
                if (search is not null) {
                    search.UpdateDate = DateTime.UtcNow.AddHours(7);
                    search.IsActive = true;
                }
            }
            else {
                _context.SearchHistories.Add(Helpers.SearchHistoryHelpers.PopulateSearchHistory(userId, searchString));
            }

            if (_context.SaveChanges() < 0) return;

            query = query.Where(x => x.Name.ToLower().Contains(searchString.ToLower()));
            if (query.Any()) {
                return;
            }
            else {
                var finalList = new List<Core.Entities.AppUser>();

                var seperateSearchStrings = searchString.Split(" ");
                if (searchString.Contains(" ")) {

                }
                else if (searchString.Contains("_")) {
                    seperateSearchStrings = searchString.Split("_");
                }
                else if (searchString.Contains("-")) {
                    seperateSearchStrings = searchString.Split("-");
                }

                foreach (var item in seperateSearchStrings) {
                    finalList = finalList.Union(_context.AppUsers.Where(x => x.Name.ToLower().Contains(item.ToLower()))
                                                                 .ToList()).ToList();

                    var listFromGame = _context.Games.Where(x => (x.DisplayName + x.Name + x.OtherName).ToLower().Contains(item.ToLower()))
                                                     .ToList();

                    var listGameOfUser = new List<Core.Entities.GameOfUser>();
                    foreach (var game in listFromGame) {
                        var list = _context.GameOfUsers.Where(x => x.GameId == game.Id).ToList();
                        listGameOfUser = listGameOfUser.Union(list).ToList();
                    }

                    foreach (var gameOfPlayer in listGameOfUser) {
                        var list = _context.AppUsers.Where(x => x.Id == gameOfPlayer.UserId).ToList();
                        finalList = finalList.Union(list).ToList();
                    }

                    // gametype
                    var listFromGameType = _context.GameTypes.Where(x => (x.Name
                                                                                + " "
                                                                                + x.ShortName
                                                                                + " "
                                                                                + x.OtherName
                                                                                + " "
                                                                                + x.Description).ToLower()
                                                                                                .Contains(item.ToLower()))
                                                            .ToList();

                    // type of game
                    var listTypeOfGame = new List<Core.Entities.TypeOfGame>();
                    foreach (var gameType in listFromGameType) {
                        var list = _context.TypeOfGames.Where(x => x.GameTypeId == gameType.Id).ToList();
                        listTypeOfGame = listTypeOfGame.Union(list).ToList();
                    }

                    // game from type of game
                    var listFromGame02 = new List<Core.Entities.Game>();
                    foreach (var typeOfGame in listTypeOfGame) {
                        var list = _context.Games.Where(x => x.Id == typeOfGame.GameId).ToList();
                        listFromGame02 = listFromGame02.Union(list).ToList();
                    }

                    // game of player from game
                    var listGameOfPlayer02 = new List<Core.Entities.GameOfUser>();
                    foreach (var game in listFromGame02) {
                        var list = _context.GameOfUsers.Where(x => x.GameId == game.Id).ToList();
                        listGameOfPlayer02 = listGameOfPlayer02.Union(list).ToList();
                    }

                    foreach (var gameOfPlayer in listGameOfPlayer02) {
                        var list = _context.AppUsers.Where(x => x.Id == gameOfPlayer.UserId).ToList();
                        finalList = finalList.Union(list).ToList();
                    }
                }
                query = finalList.AsQueryable();
            }
        }

        private void FilterByStatus(ref IQueryable<Core.Entities.AppUser> query, string status)
        {
            if (!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)) {
                return;
            }
            query = query.Where(x => x.Status.ToLower().Contains(status.ToLower()));
        }

        public async Task<Result<bool>> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request)
        {
            var result = new Result<bool>();
            var user = await _context.AppUsers.FindAsync(userId);

            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (request.IsActive == true) {
                if (user.IsActive == true) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Tài khoản user {user.Name} hiện đang active.");
                    return result;
                }
                user.IsActive = true;

                var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsActive == true);
                if (disable is not null) {
                    disable.IsActive = false;
                }
                if (await _context.SaveChangesAsync() >= 0) {
                    result.Content = true;
                    return result;
                }
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }
            else {
                if (user.IsActive == false) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Tài khoản user {user.Name} hiện đang bị khóa.");
                    return result;
                }
                if (String.IsNullOrEmpty(request.Note) || String.IsNullOrWhiteSpace(request.Note)) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Bạn chưa nhập lí do khóa tài khoản.");
                    return result;
                }
                if (request.NumDateDisable <= 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Số ngày khóa tài khoản nhập không phù hợp. Vui lòng kiểm tra và nhập lại.");
                    return result;
                }
                if (request.DateDisable.Year == 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Thời gian khóa tài khoản nhập không phù hợp. Vui lòng kiểm tra và nhập lại.");
                    return result;
                }

                if (request.DateActive.Year == 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Thời gian tài khoản được mở lại nhập không phù hợp. Vui lòng kiểm tra và nhập lại.");
                    return result;
                }

                await _context.DisableUsers.AddAsync(
                    Helpers.DisableUserHelpers.PopulateDisableUser(user.Id, request.DateDisable, request.DateActive, request.Note, request.NumDateDisable)
                );

                await _context.Entry(user).Collection(x => x.Orders).LoadAsync();
                var orders = user.Orders.Where(x => x.Status == OrderStatusConstants.Processing);
                if (orders.Count() > 0) {
                    foreach (var order in orders) {
                        await _context.Entry(order).Reference(x => x.User).LoadAsync();
                        order.Status = OrderStatusConstants.Interrupt;
                        if(order.UserId == user.Id){
                            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
                            toUser.Status = UserStatusConstants.Online;
                        }else{
                            order.User.Status = UserStatusConstants.Online;
                        }
                    }
                }

                if (await _context.SaveChangesAsync() >= 0) {
                    user.IsActive = false;
                    await _context.Notifications.AddAsync(
                            Helpers.NotificationHelpers.PopulateNotification(
                            userId, $"Xin chào {user.Name}, bạn đã bị khóa tài khoản {request.NumDateDisable} ngày!!!",
                            $"Vì: {request.Note}. Tài khoản của bạn sẽ mở lại lúc {request.DateActive}!!!. Bắt đầu khóa lúc {request.DateDisable}. Chào bạn!!!",
                            ""
                        )
                    );
                    if (await _context.SaveChangesAsync() >= 0) {
                        result.Content = true;
                        return result;
                    }
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Có lỗi xảy ra. Vui lòng thử lại.");
            return result;

        }

        public async Task<Result<DisableUserResponse>> GetDisableInfoAsync(ClaimsPrincipal principal)
        {
            var result = new Result<DisableUserResponse>();
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

            var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsActive == true);
            if (disable is not null) {
                var response = _mapper.Map<DisableUserResponse>(disable);
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy thông tin khóa tài khoản.");
            return result;
        }

        public async Task<Result<bool>> ActiveUserAsync(ClaimsPrincipal principal)
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

            var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsActive == true);
            if (disable is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy thông tin khóa tài khoản.");
                return result;
            }
            if (DateTime.UtcNow.AddHours(7) > disable.DateActive) {
                disable.IsActive = false;
                user.IsActive = true;
                if (await _context.SaveChangesAsync() >= 0) {
                    result.Content = true;
                    return result;
                }
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Tài khoản của bạn sẽ được mở lại lúc {disable.DateActive}.");
            return result;
        }

        public async Task<Result<bool>> TurnOnIsPlayerWhenDatingComeOnAsync()
        {
            var result = new Result<bool>();
            var users = await _context.AppUsers.ToListAsync();
            foreach (var item in users) {
                if (item.IsActive is false) {
                    continue;
                }

                if (item.PricePerHour < ValueConstants.PricePerHourMinValue) {
                    continue;
                }

                if (item.MaxHourHire < ValueConstants.MaxHourHireMinValue
                    || item.MaxHourHire > ValueConstants.MaxHourHireMaxValue) {
                    continue;
                }

                var gameOfUserExist = await _context.GameOfUsers.AnyAsync(x => x.UserId == item.Id);
                if (!gameOfUserExist) {
                    continue;
                }

                if (item.IsPlayer is true) {
                    continue;
                }

                var datings = await _context.Datings.Where(x => x.UserId == item.Id).ToListAsync();
                if (datings.Count == 0) {
                    continue;
                }
                int toDay = CalculateDayInWeekToday(DateTime.UtcNow.AddHours(7).DayOfWeek);
                var toDayDate = datings.Where(x => x.DayInWeek == toDay).ToList();
                if (toDayDate.Count == 0) {
                    continue;
                }
                var hourNow = DateTime.UtcNow.AddHours(7).Hour;
                var minuteNow = DateTime.UtcNow.AddHours(7).Minute;
                var timeNow = hourNow * 60 + minuteNow;
                var existDatingNow = toDayDate.Any(x => x.FromHour <= timeNow && timeNow <= x.ToHour);
                if (existDatingNow) {
                    item.IsPlayer = true;
                }
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }

            }
            result.Content = true;
            return result;
        }

        public async Task<Result<BehaviorPointResponse>> GetBehaviorPointAsync(string userId)
        {
            var result = new Result<BehaviorPointResponse>();
            var user = await _context.AppUsers.FindAsync(userId);

            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            await _context.Entry(user).Reference(x => x.BehaviorPoint).LoadAsync();
            var response = _mapper.Map<BehaviorPointResponse>(user.BehaviorPoint);
            result.Content = response;
            return result;
        }

        public async Task<Result<UserBalanceResponse>> GetBalanceAsync(string userId)
        {
            var result = new Result<UserBalanceResponse>();
            var user = await _context.AppUsers.FindAsync(userId);

            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();
            var response = _mapper.Map<UserBalanceResponse>(user.UserBalance);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateRankingPointAsync()
        {
            var result = new Result<bool>();
            var listUser = await _context.AppUsers.ToListAsync();
            foreach (var item in listUser) {
                if (item.Rate == 0 || item.NumOfRate == 0) {
                    continue;
                }
                var date = Math.Ceiling(Helpers.UtilsHelpers.GetDayDiffer(item.CreatedDate));
                var point = (float) (item.Rate * item.NumOfRate - date);
                item.RankingPoint = point;
            }
            _context.AppUsers.UpdateRange(listUser);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> UserWithdrawMoneyAsync(ClaimsPrincipal principal, UserWithdrawMoneyRequest request)
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

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();

            if (user.UserBalance.ActiveBalance < request.MoneyWithdraw) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Số dư khả dụng ít hơn số tiền bạn muốn rút.");
                return result;
            }
            user.UserBalance.Balance -= request.MoneyWithdraw;
            user.UserBalance.ActiveBalance -= request.MoneyWithdraw;

            await _context.TransactionHistories.AddAsync(
                Helpers.TransactionHelpers.PopulateTransactionHistory(
                    user.UserBalance.Id, TransactionTypeConstants.Sub, request.MoneyWithdraw, TransactionTypeConstants.Withdraw, request.PhoneNumberMomo
                )
            );

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<(float, float, float, float)>> UserGetStatisticAsync(ClaimsPrincipal principal)
        {
            var result = new Result<(float, float, float, float)>();
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

            float dayIncome = 0;
            float monthIncome = 0;
            float percentCompleteInDay = 0;
            float percentCompleteInMonth = 0;

            var ordersInDay = await _context.Orders.Where(
                x => x.ToUserId == user.Id
                && (x.Status == OrderStatusConstants.FinishSoonHirer
                    || x.Status == OrderStatusConstants.FinishSoonPlayer
                    || x.Status == OrderStatusConstants.Complete)
                && (x.CreatedDate.Day == DateTime.UtcNow.AddHours(7).Day
                    && x.CreatedDate.Month == DateTime.UtcNow.AddHours(7).Month
                    && x.CreatedDate.Year == DateTime.UtcNow.AddHours(7).Year))
                .ToListAsync();
            dayIncome = (from order in ordersInDay select order.FinalPrices).Sum();
            if (ordersInDay.Count() != 0) {
                percentCompleteInDay = (from order in ordersInDay where order.Status == OrderStatusConstants.Complete select order).Count() * 100 / ordersInDay.Count();
            }


            var ordersInMonth = await _context.Orders.Where(
                x => x.ToUserId == user.Id
                && (x.Status == OrderStatusConstants.FinishSoonHirer
                    || x.Status == OrderStatusConstants.FinishSoonPlayer
                    || x.Status == OrderStatusConstants.Complete)
                && (x.CreatedDate.Month == DateTime.UtcNow.AddHours(7).Month
                    && x.CreatedDate.Year == DateTime.UtcNow.AddHours(7).Year))
                .ToListAsync();
            monthIncome = (from order in ordersInMonth select order.FinalPrices).Sum();
            if (ordersInMonth.Count() != 0) {
                percentCompleteInMonth = (from order in ordersInMonth where order.Status == OrderStatusConstants.Complete select order).Count() * 100 / ordersInMonth.Count();
            }
            result.Content = (dayIncome, monthIncome, percentCompleteInDay, percentCompleteInMonth);
            return result;
        }
    }
}