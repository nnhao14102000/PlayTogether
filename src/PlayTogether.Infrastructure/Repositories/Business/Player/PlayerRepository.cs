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

        public async Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(ClaimsPrincipal principal, PlayerParameters param)
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

            FilterActivePlayer(ref queryPlayer);
            FilterPlayerStatus(ref queryPlayer, param.PlayerStatus);

            FilterPlayerRecentHired(ref queryPlayer, param.IsRecent, hirer.Id);
            FilterPlayerByGameId(ref queryPlayer, param.GameId);
            FilterPlayerByMusicId(ref queryPlayer, param.MusicId);
            FilterPlayerByGender(ref queryPlayer, param.Gender);

            SearchPlayerByName(ref queryPlayer, param.Name);

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
            var orders = _context.Orders.Where(x => x.HirerId == hirerId)
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

        private void FilterActivePlayer(ref IQueryable<Entities.Player> queryPlayer)
        {
            if (!queryPlayer.Any()) {
                return;
            }
            queryPlayer = queryPlayer.Where(x => x.IsActive == true);
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
            queryPlayer = queryPlayer.OrderByDescending(x => x.Rating);
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

        private void SearchPlayerByName(
            ref IQueryable<Entities.Player> players,
            string name)
        {
            if (!players.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            players = players.Where(x => (x.Lastname + x.Firstname).ToLower()
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

    }
}
