using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Player
{
    public class PlayerRepository : BaseRepository, IPlayerRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PlayerRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }



        /// <summary>
        /// Load lại cho Hirer những người mà họ đã thuê gần đây
        /// </summary>
        /// <param name="queryPlayer"></param>
        /// <param name="isRecent"></param>
        /// <param name="hireId"></param>
        private void FilterPlayerRecentHired(ref IQueryable<Entities.Player> queryPlayer, bool? isRecent, string hirerId)
        {
            if ((!queryPlayer.Any())
                || isRecent is null
                || isRecent is false
                || String.IsNullOrEmpty(hirerId)
                || String.IsNullOrWhiteSpace(hirerId)) {
                return;
            }
            var orders = _context.Orders.Where(x => x.HirerId == hirerId && x.Status == OrderStatusConstants.Complete)
                                        .OrderByDescending(x => x.CreatedDate)
                                        .ToList();
            List<Entities.Player> players = new();
            foreach (var item in orders) {
                players.Add(item.Player);
            }
            queryPlayer = players.AsQueryable().Distinct();
        }

        private void FilterPlayerStatus(ref IQueryable<Entities.Player> queryPlayer, string playerStatus)
        {
            if (!queryPlayer.Any() || String.IsNullOrEmpty(playerStatus) || String.IsNullOrWhiteSpace(playerStatus)) {
                return;
            }
            queryPlayer = queryPlayer.Where(x => x.Status.ToLower() == playerStatus.ToLower());
        }

        private void FilterActivePlayer(ref IQueryable<Entities.Player> queryPlayer, bool? isActive)
        {
            if (!queryPlayer.Any() || isActive is null) {
                return;
            }
            queryPlayer = queryPlayer.Where(x => x.IsActive == isActive);
        }

        private void OrderPlayerByLowestPricing(ref IQueryable<Entities.Player> queryPlayer, bool? isOrderByPricing)
        {
            if (!queryPlayer.Any() || isOrderByPricing is null || isOrderByPricing is false) {
                return;
            }
            queryPlayer = queryPlayer.OrderByDescending(x => x.PricePerHour);
        }

        private void OrderPlayerByHighestRating(ref IQueryable<Entities.Player> queryPlayer, bool? isOrderByRating)
        {
            if (!queryPlayer.Any() || isOrderByRating is null || isOrderByRating is false) {
                return;
            }
            queryPlayer = queryPlayer.OrderByDescending(x => x.Rate);
        }

        private void OrderPlayerByASCName(ref IQueryable<Entities.Player> queryPlayer, bool? isOrderByFirstName)
        {
            if (!queryPlayer.Any() || isOrderByFirstName is null) {
                return;
            }
            if (isOrderByFirstName == true) {
                queryPlayer = queryPlayer.OrderBy(x => x.Firstname);
            }
            else {
                queryPlayer = queryPlayer.OrderByDescending(x => x.Firstname);
            }
        }

        private void FilterPlayerByGender(
            ref IQueryable<Entities.Player> players,
            bool? gender)
        {
            if (!players.Any() || gender is null) {
                return;
            }
            players = players.Where(x => x.Gender == gender);
        }

        private void FilterPlayerByGameId(
            ref IQueryable<Entities.Player> players,
            string gameId)
        {
            if (!players.Any() || String.IsNullOrEmpty(gameId) || String.IsNullOrWhiteSpace(gameId)) {
                return;
            }
            List<Entities.Player> result = new();
            var gameOfPlayer = _context.GameOfPlayers.Where(x => x.GameId == gameId);
            foreach (var item in gameOfPlayer) {
                result.Add(item.Player);
            }
            players = result.AsQueryable();
        }

        private void FilterPlayerByMusicId(
            ref IQueryable<Entities.Player> players,
            string musicId)
        {
            if (!players.Any() || String.IsNullOrEmpty(musicId) || String.IsNullOrWhiteSpace(musicId)) {
                return;
            }
            List<Entities.Player> result = new();
            var musicOfPlayer = _context.MusicOfPlayers.Where(x => x.MusicId == musicId);
            foreach (var item in musicOfPlayer) {
                result.Add(item.Player);
            }
            players = result.AsQueryable();
        }

        private void FilterPlayerByName(
            ref IQueryable<Entities.Player> query,
            string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => (x.Lastname + " " + x.Firstname).ToLower()
                                                                   .Contains(name.ToLower()));
        }

        public async Task<PlayerGetByIdResponseForHirer> GetPlayerByIdForHirerAsync(string id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player is null) {
                return null;
            }

            _context.Entry(player)
                .Collection(p => p.Images)
                .Query()
                .OrderByDescending(i => i.CreatedDate)
                .Load();

            return _mapper.Map<PlayerGetByIdResponseForHirer>(player);
        }

        public async Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdForPlayerAsync(string id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player is null) {
                return null;
            }

            await _context.Entry(player)
                .Collection(p => p.Images)
                .Query()
                .OrderByDescending(i => i.CreatedDate)
                .LoadAsync();
            return _mapper.Map<PlayerGetByIdResponseForPlayer>(player);
        }

        public async Task<PlayerOtherSkillResponse> GetPlayerOtherSkillByIdAsync(string id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player is null) {
                return null;
            }
            return _mapper.Map<PlayerOtherSkillResponse>(player);
        }

        public async Task<PlayerGetProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var playerProfile = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            return _mapper.Map<PlayerGetProfileResponse>(playerProfile);
        }

        public async Task<PlayerServiceInfoResponseForPlayer> GetPlayerServiceInfoByIdForPlayerAsync(string id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player is null) {
                return null;
            }
            return _mapper.Map<PlayerServiceInfoResponseForPlayer>(player);
        }

        public async Task<bool> UpdatePlayerInformationAsync(string id, PlayerInfoUpdateRequest request)
        {
            var player = await _context.Players.FindAsync(id);
            if (player is null) {
                return false;
            }
            _mapper.Map(request, player);
            _context.Players.Update(player);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdatePlayerOtherSkillAsync(string id, OtherSkillUpdateRequest request)
        {
            var player = await _context.Players.FindAsync(id);
            if (player is null) {
                return false;
            }
            _mapper.Map(request, player);
            _context.Players.Update(player);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdatePlayerServiceInfoAsync(string id, PlayerServiceInfoUpdateRequest request)
        {
            var player = await _context.Players.FindAsync(id);
            if (player is null || player.Status is PlayerStatusConstants.NotAcceptPolicy) {
                return false;
            }

            if (request.Status) {
                player.Status = PlayerStatusConstants.Online;
            }
            else {
                player.Status = PlayerStatusConstants.Offline;
            }
            player.MaxHourHire = request.MaxHourHire;
            player.PricePerHour = request.PricePerHour;

            _context.Players.Update(player);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> AcceptPolicyAsync(ClaimsPrincipal principal, PlayerAcceptPolicyRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (player is null || player.Status is not PlayerStatusConstants.NotAcceptPolicy || player.IsActive is false) {
                return false;
            }

            if (request.Accept is false) {
                return true;
            }

            player.Status = PlayerStatusConstants.Offline;
            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(
            ClaimsPrincipal principal,
            PlayerParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (hirer is null || hirer.IsActive is false) {
                return null;
            }

            var players = await _context.Players.ToListAsync();

            var queryPlayer = players.AsQueryable();

            FilterActivePlayer(ref queryPlayer, true);

            Search(ref queryPlayer, param.SearchString);

            FilterPlayerStatus(ref queryPlayer, param.Status);
            FilterPlayerByName(ref queryPlayer, param.Name);
            FilterPlayerRecentHired(ref queryPlayer, param.IsRecentOrder, hirer.Id);
            FilterPlayerByGameId(ref queryPlayer, param.GameId);
            FilterPlayerByMusicId(ref queryPlayer, param.MusicId);
            FilterPlayerByGender(ref queryPlayer, param.Gender);

            OrderPlayerByASCName(ref queryPlayer, param.IsOrderByFirstName);
            OrderPlayerByHighestRating(ref queryPlayer, param.IsOrderByRating);
            OrderPlayerByLowestPricing(ref queryPlayer, param.IsOrderByPricing);

            players = queryPlayer.ToList();
            var response = _mapper.Map<List<PlayerGetAllResponseForHirer>>(players);
            return PagedResult<PlayerGetAllResponseForHirer>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);

        }

        private void Search(ref IQueryable<Entities.Player> queryPlayer, string searchString)
        {
            if (String.IsNullOrEmpty(searchString) || String.IsNullOrWhiteSpace(searchString)) {
                return;
            }

            var finalList = new List<Entities.Player>();

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
                finalList = finalList.Union(_context.Players.Where(x => (x.Firstname
                                                                    + x.Lastname).ToLower()
                                                                                   .Contains(item.ToLower()))
                                                            .ToList()).ToList();

                var listFromGame = _context.Games.Where(x => (x.DisplayName
                                                                    + x.Name
                                                                    + x.OtherName).ToLower()
                                                                                  .Contains(item.ToLower()))
                                                                .ToList();

                var listGameOfPlayer = new List<Entities.GameOfPlayer>();
                foreach (var game in listFromGame) {
                    var list = _context.GameOfPlayers.Where(x => x.GameId == game.Id).ToList();
                    listGameOfPlayer = listGameOfPlayer.Union(list).ToList();
                }

                foreach (var gameOfPlayer in listGameOfPlayer) {
                    var list = _context.Players.Where(x => x.Id == gameOfPlayer.PlayerId).ToList();
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
                var listGameOfPlayer02 = new List<Entities.GameOfPlayer>();
                foreach (var game in listFromGame02) {
                    var list = _context.GameOfPlayers.Where(x => x.GameId == game.Id).ToList();
                    listGameOfPlayer02 = listGameOfPlayer02.Union(list).ToList();
                }

                foreach (var gameOfPlayer in listGameOfPlayer02) {
                    var list = _context.Players.Where(x => x.Id == gameOfPlayer.PlayerId).ToList();
                    finalList = finalList.Union(list).ToList();
                }
            }
            queryPlayer = finalList.AsQueryable();
        }

        public async Task<PagedResult<PlayerGetAllResponseForAdmin>> GetAllPlayersForAdminAsync(PlayerForAdminParameters param)
        {
            var players = await _context.Players.ToListAsync();

            var queryPlayer = players.AsQueryable();

            FilterActivePlayer(ref queryPlayer, param.IsActive);
            FilterPlayerStatus(ref queryPlayer, param.Status);
            FilterPlayerByName(ref queryPlayer, param.Name);

            players = queryPlayer.ToList();
            var response = _mapper.Map<List<PlayerGetAllResponseForAdmin>>(players);
            return PagedResult<PlayerGetAllResponseForAdmin>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        public async Task<PlayerGetByIdForAdminResponse> GetPlayerByIdForAdminAsync(string playerId)
        {
            var player = await _context.Players.FindAsync(playerId);

            if (player is null) {
                return null;
            }
            
            return _mapper.Map<PlayerGetByIdForAdminResponse>(player);
        }

        public async Task<bool> UpdatePlayerStatusForAdminAsync(string playerId, PlayerStatusUpdateRequest request)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player is null) {
                return false;
            }
            _mapper.Map(request, player);
            _context.Players.Update(player);
            if (await _context.SaveChangesAsync() < 0) {
                return false;
            }
            if (request.IsActive is false) {
                player.Status = PlayerStatusConstants.Offline;
                await _context.Entry(player).Collection(x => x.Orders).LoadAsync();
                var orders = player.Orders.Where(x => x.Status == OrderStatusConstants.Processing);
                if (orders.Count() > 0) {
                    foreach (var order in orders) {
                        await _context.Entry(order).Reference(x => x.Player).LoadAsync();
                        order.Status = OrderStatusConstants.Interrupt;
                        order.Player.Status = PlayerStatusConstants.Online;
                    }
                }
                await _context.Notifications.AddAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = player.Id,
                        Title = $"Tài khoản của bạn đã bị khóa.😅",
                        Message = (String.IsNullOrEmpty(request.Message) || String.IsNullOrWhiteSpace(request.Message)) ? $"Bạn đã bị khóa tài khoản vì bạn đã đã hành vi không thích hợp. Hạn khóa tài khoản là đến ngày {DateTime.Now.AddDays(1)}" : $"Bạn đã bị khóa tài khoản vì: \"{request.Message}\". Hạn khóa tài khoản là đến ngày {DateTime.Now.AddDays(1)}",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
                return (await _context.SaveChangesAsync() >= 0);
            }
            else {
                return (await _context.SaveChangesAsync() >= 0);
            }
        }
    }
}
