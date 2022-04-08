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

        public async Task<bool> ChangeIsPlayerAsync(ClaimsPrincipal principal, UserIsPlayerChangeRequest request)
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

            if (request.IsPlayer is true) {
                if (user.IsActive is false) return false;

                if (user.Status is UserStatusConstants.Offline) return false;

                if (user.PricePerHour < ValueConstants.PricePerHourMinValue) return false;

                if (user.MaxHourHire < ValueConstants.MaxHourHireMinValue
                    || user.MaxHourHire > ValueConstants.MaxHourHireMaxValue) return false;

                var gameOfUserExist = await _context.GameOfUsers.AnyAsync(x => x.UserId == user.Id);
                if (!gameOfUserExist) return false;

            }
            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PersonalInfoResponse> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal)
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
            await _context.Entry(user).Collection(x => x.Images).LoadAsync();
            await _context.Entry(user).Collection(x => x.Datings).LoadAsync();
            await _context.Entry(user).Reference(x => x.BehaviorPoint).LoadAsync();

            var rates = await _context.Ratings.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orders = await _context.Orders.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orderOnTimes = await _context.Orders.Where(x => x.ToUserId == user.Id
                                                                && x.Status == OrderStatusConstants.Complete).ToListAsync();
            double totalTime = 0;
            foreach (var item in orders) {
                totalTime += Helpers.UtilsHelpers.GetTime(item.TimeStart, item.TimeFinish);
            }
            user.NumOfRate = rates.Count();
            user.NumOfOrder = orders.Count();
            user.TotalTimeOrder = Convert.ToInt32(Math.Ceiling(totalTime / 3600));
            user.NumOfFinishOnTime = orderOnTimes.Count();

            return _mapper.Map<PersonalInfoResponse>(user);
        }

        public async Task<bool> UpdateUserServiceInfoAsync(ClaimsPrincipal principal, UserInfoForIsPlayerUpdateRequest request)
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

            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request)
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

            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<UserGetBasicInfoResponse> GetUserBasicInfoByIdAsync(string userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                return null;
            }
            await _context.Entry(user).Collection(x => x.Images).LoadAsync();
            await _context.Entry(user).Collection(x => x.Datings).LoadAsync();
            await _context.Entry(user).Reference(x => x.BehaviorPoint).LoadAsync();

            var rates = await _context.Ratings.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orders = await _context.Orders.Where(x => x.ToUserId == user.Id).ToListAsync();
            var orderOnTimes = await _context.Orders.Where(x => x.ToUserId == user.Id
                                                                && x.Status == OrderStatusConstants.Complete).ToListAsync();
            double totalTime = 0;
            foreach (var item in orders) {
                totalTime += Helpers.UtilsHelpers.GetTime(item.TimeStart, item.TimeFinish);
            }
            user.NumOfRate = rates.Count();
            user.NumOfOrder = orders.Count();
            user.TotalTimeOrder = Convert.ToInt32(Math.Ceiling(totalTime / 3600));
            user.NumOfFinishOnTime = orderOnTimes.Count();

            var response = _mapper.Map<UserGetBasicInfoResponse>(user);
            return response;
        }

        public async Task<UserGetServiceInfoResponse> GetUserServiceInfoByIdAsync(string userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                return null;
            }
            return _mapper.Map<UserGetServiceInfoResponse>(user);
        }

        public async Task<PagedResult<UserSearchResponse>> GetAllUsersAsync(
            ClaimsPrincipal principal,
            UserParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (user is null || user.IsActive is false || user.Status == UserStatusConstants.Offline) {
                return null;
            }

            var users = await _context.AppUsers.ToListAsync();

            var query = users.AsQueryable();

            Search(ref query, user.Id, param.Search);
            FilterActiveUser(ref query, true);
            FilterIsPlayerUser(ref query, param.IsPlayer);
            FilterUserStatus(ref query, param.Status);
            FilterUserByName(ref query, user.Id, param.Name);
            FilterUserRecentHired(ref query, param.IsRecentOrder, user.Id);
            FilterHaveSkillSameHobby(ref query, param.IsSameHobbies, user);
            FilterUserByGameId(ref query, param.GameId);
            FilterUserByGender(ref query, param.Gender);
            FilterUserByHour(ref query, param.FromHour, param.ToHour);
            FilterUserByDate(ref query, param.Date);

            FilterUserByItSelf(ref query, user.Id);

            OrderUserByASCName(ref query, param.IsOrderByName);
            OrderUserByHighestRating(ref query, param.IsOrderByRating);
            OrderUserPricing(ref query, param.IsOrderByPricing);
            OrderUserByCreatedDate(ref query, param.IsNewAccount);

            users = query.ToList();
            var response = _mapper.Map<List<UserSearchResponse>>(users);
            foreach (var item in response) {
                var rates = await _context.Ratings.Where(x => x.ToUserId == item.Id).ToListAsync();
                item.NumOfRate = rates.Count();
            }
            return PagedResult<UserSearchResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterUserByDate(ref IQueryable<Entities.AppUser> query, string date)
        {
            if (!query.Any()
               || String.IsNullOrEmpty(date)
               || String.IsNullOrWhiteSpace(date)) {
                return;
            }
            var list = new List<Entities.AppUser>();
            if (date.ToLower() is "mon") {
                var datings = _context.Datings.Where(x => x.IsMON == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (date.ToLower() is "tue") {
                var datings = _context.Datings.Where(x => x.IsTUE == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (date.ToLower() is "wed") {
                var datings = _context.Datings.Where(x => x.IsWED == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (date.ToLower() is "thu") {
                var datings = _context.Datings.Where(x => x.IsTHU == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (date.ToLower() is "fri") {
                var datings = _context.Datings.Where(x => x.IsFRI == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (date.ToLower() is "sat") {
                var datings = _context.Datings.Where(x => x.IsSAT == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            if (date.ToLower() is "sun") {
                var datings = _context.Datings.Where(x => x.IsSUN == true);
                foreach (var item in datings) {
                    var user = _context.AppUsers.Find(item.UserId);
                    list.Add(user);
                }
            }
            query = list.AsQueryable();
        }

        private void FilterUserByHour(ref IQueryable<Entities.AppUser> query, string fromHour, string toHour)
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
            var list = new List<Entities.AppUser>();
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

            users = query.ToList();
            var response = _mapper.Map<List<UserGetByAdminResponse>>(users);
            return PagedResult<UserGetByAdminResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void OrderUserByCreatedDate(ref IQueryable<Entities.AppUser> query, bool? isNewAccount)
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
            ref IQueryable<Entities.AppUser> query,
            bool? isSameHobbies,
            Entities.AppUser user)
        {
            if (!query.Any() || isSameHobbies is false || isSameHobbies is null) {
                return;
            }

            var final = new List<Entities.AppUser>();
            var listGameOfUser = new List<Entities.GameOfUser>();

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

        private void FilterUserByItSelf(ref IQueryable<Entities.AppUser> query, string id)
        {
            if (!query.Any() || String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id)) {
                return;
            }
            query = query.Where(x => x.Id != id);
        }

        private void OrderUserPricing(ref IQueryable<Entities.AppUser> query, bool? isOrderByPricing)
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

        private void OrderUserByHighestRating(ref IQueryable<Entities.AppUser> query, bool? isOrderByRating)
        {
            if (!query.Any() || isOrderByRating is null || isOrderByRating is false) {
                return;
            }
            query = query.OrderBy(x => x.CreatedDate).ThenByDescending(x => x.NumOfRate).ThenByDescending(x => x.Rate);
        }

        private void OrderUserByASCName(ref IQueryable<Entities.AppUser> query, bool? isOrderByName)
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
            ref IQueryable<Entities.AppUser> query,
            bool? gender)
        {
            if (!query.Any() || gender is null) {
                return;
            }
            query = query.Where(x => x.Gender == gender);
        }

        private void FilterUserByGameId(
            ref IQueryable<Entities.AppUser> query,
            string gameId)
        {
            if (!query.Any() || String.IsNullOrEmpty(gameId) || String.IsNullOrWhiteSpace(gameId)) {
                return;
            }
            List<Entities.AppUser> result = new();
            var gamesOfUser = _context.GameOfUsers.Where(x => x.GameId == gameId);
            foreach (var item in gamesOfUser) {
                result.Add(item.User);
            }
            query = result.AsQueryable();
        }

        private void FilterUserRecentHired(ref IQueryable<Entities.AppUser> query, bool? isRecent, string userId)
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
            List<Entities.AppUser> players = new();
            foreach (var item in orders) {
                // _context.Entry(item).Reference(x => x.User).Load();
                var player = _context.AppUsers.Find(item.ToUserId);
                players.Add(player);
            }
            query = players.AsQueryable().Distinct();
        }

        private void FilterUserByName(
            ref IQueryable<Entities.AppUser> query,
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
            ref IQueryable<Entities.AppUser> query,
            string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => (x.Name.ToLower() + " " + x.Email.ToLower()).Contains(name.ToLower()));
        }

        private void FilterUserStatus(ref IQueryable<Entities.AppUser> query, string userStatus)
        {
            if (!query.Any() || String.IsNullOrEmpty(userStatus) || String.IsNullOrWhiteSpace(userStatus)) {
                return;
            }
            query = query.Where(x => x.Status.ToLower() == userStatus.ToLower());
        }

        private void FilterIsPlayerUser(ref IQueryable<Entities.AppUser> query, bool? isPlayer)
        {
            if (!query.Any() || isPlayer is null) {
                return;
            }
            query = query.Where(x => x.IsPlayer == isPlayer);
        }

        private void FilterActiveUser(ref IQueryable<Entities.AppUser> query, bool? isActive)
        {
            if (!query.Any() || isActive is null) {
                return;
            }
            query = query.Where(x => x.IsActive == isActive);
        }

        private void Search(ref IQueryable<Entities.AppUser> query, string userId, string searchString)
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

            var finalList = new List<Entities.AppUser>();

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

                var listGameOfUser = new List<Entities.GameOfUser>();
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
                var listTypeOfGame = new List<Entities.TypeOfGame>();
                foreach (var gameType in listFromGameType) {
                    var list = _context.TypeOfGames.Where(x => x.GameTypeId == gameType.Id).ToList();
                    listTypeOfGame = listTypeOfGame.Union(list).ToList();
                }

                // game from type of game
                var listFromGame02 = new List<Entities.Game>();
                foreach (var typeOfGame in listTypeOfGame) {
                    var list = _context.Games.Where(x => x.Id == typeOfGame.GameId).ToList();
                    listFromGame02 = listFromGame02.Union(list).ToList();
                }

                // game of player from game
                var listGameOfPlayer02 = new List<Entities.GameOfUser>();
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



        private void FilterByStatus(ref IQueryable<Entities.AppUser> query, string status)
        {
            if (!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)) {
                return;
            }
            query = query.Where(x => x.Status.ToLower().Contains(status.ToLower()));
        }

        public async Task<bool> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request)
        {
            var user = await _context.AppUsers.FindAsync(userId);

            if (user is null) {
                return false;
            }

            if (request.IsActive == true) {
                if (user.IsActive == true) {
                    return false;
                }
                user.IsActive = true;

                var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsActive == true);
                if (disable is not null) {
                    disable.IsActive = false;
                }
                return await _context.SaveChangesAsync() >= 0;
            }
            else {
                if (user.IsActive == false) {
                    return false;
                }
                if (String.IsNullOrEmpty(request.Note) || String.IsNullOrWhiteSpace(request.Note)) {
                    return false;
                }
                if (request.NumDateDisable <= 0) {
                    return false;
                }
                if (request.DateDisable.Year == 0 || request.DateActive.Year == 0) {
                    return false;
                }

                await _context.DisableUsers.AddAsync(
                    Helpers.DisableUserHelpers.PopulateDisableUser(user.Id, request.DateDisable, request.DateActive, request.Note, request.NumDateDisable)
                );

                if (await _context.SaveChangesAsync() >= 0) {
                    user.IsActive = false;
                    await _context.Notifications.AddAsync(
                            Helpers.NotificationHelpers.PopulateNotification(
                            userId, $"Xin chào {user.Name}, bạn đã bị khóa tài khoản {request.NumDateDisable} ngày!!!",
                            $"Vì: {request.Note}. Tài khoản của bạn sẽ mở lại lúc {request.DateActive}!!!. Bắt đầu khóa lúc {request.DateDisable}. Chào bạn!!!",
                            ""
                        )
                    );
                    return await _context.SaveChangesAsync() >= 0;
                }
            }

            return false;

        }

        public async Task<DisableUserResponse> GetDisableInfoAsync(ClaimsPrincipal principal)
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

            var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsActive == true);
            if (disable is not null) {
                return _mapper.Map<DisableUserResponse>(disable);
            }
            return null;
        }

        public async Task<bool> ActiveUserAsync(ClaimsPrincipal principal)
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

            var disable = await _context.DisableUsers.FirstOrDefaultAsync(x => x.UserId == user.Id && x.IsActive == true);
            if (disable is null) {
                return false;
            }
            if (DateTime.UtcNow.AddHours(7) > disable.DateActive) {
                disable.IsActive = false;
                user.IsActive = true;
                return await _context.SaveChangesAsync() >= 0;
            }
            return false;
        }
    }
}