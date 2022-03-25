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
            return _mapper.Map<UserGetBasicInfoResponse>(user);
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

            FilterActiveUser(ref query, true);
            FilterIsPlayerUser(ref query, true);

            Search(ref query, user.Id, param.SearchString);

            FilterUserStatus(ref query, param.Status);
            FilterUserByName(ref query, param.Name);
            // FilterUserRecentHired(ref query, param.IsRecentOrder, user.Id);
            FilterUserByGameId(ref query, param.GameId);
            FilterUserByGender(ref query, param.Gender);

            FilterUserByItSelf(ref query, user.Id);

            OrderUserByASCName(ref query, param.IsOrderByName);
            OrderUserByHighestRating(ref query, param.IsOrderByRating);
            OrderUserPricing(ref query, param.IsOrderByPricing);

            users = query.ToList();
            var response = _mapper.Map<List<UserSearchResponse>>(users);
            return PagedResult<UserSearchResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
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
            query = query.OrderByDescending(x => x.Rate);
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

        // private void FilterUserRecentHired(ref IQueryable<Entities.AppUser> query, bool? isRecent, string userId)
        // {
        //     if ((!query.Any())
        //         || isRecent is null
        //         || isRecent is false
        //         || String.IsNullOrEmpty(userId)
        //         || String.IsNullOrWhiteSpace(userId)) {
        //         return;
        //     }
        //     var orders = _context.Orders.Where(x => x.UserId == userId && x.Status == OrderStatusConstants.Complete)
        //                                 .OrderByDescending(x => x.CreatedDate)
        //                                 .ToList();
        //     List<Entities.AppUser> players = new();
        //     foreach (var item in orders) {
        //         players.Add(item.User);
        //     }
        //     query = players.AsQueryable().Distinct();
        // }

        private void FilterUserByName(
            ref IQueryable<Entities.AppUser> query,
            string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
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

            _context.SearchHistories.Add(Helpers.SearchHistoryHelpers.PopulateSearchHistory(userId, searchString));
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

        public async Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param)
        {
            var users = await _context.AppUsers.ToListAsync();

            var query = users.AsQueryable();

            FilterActiveUser(ref query, param.IsActive);
            FilterByStatus(ref query, param.Status);
            

            users = query.ToList();
            var response = _mapper.Map<List<UserGetByAdminResponse>>(users);
            return PagedResult<UserGetByAdminResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterByStatus(ref IQueryable<Entities.AppUser> query, string status)
        {
            if(!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)){
                return ;
            }
            query = query.Where(x => x.Status.ToLower().Contains(status.ToLower()));
        }
    }
}