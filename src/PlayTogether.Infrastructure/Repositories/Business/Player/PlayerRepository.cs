using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Player;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PlayerRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(PlayerParameters param)
        {
            List<Entities.Player> players = null;

            if (param.Gender is not null) {
                players = await _context.Players.Where(x => x.Gender == param.Gender).ToListAsync();
            }
            else {

                players = await _context.Players.ToListAsync().ConfigureAwait(false);
            }

            if (!String.IsNullOrEmpty(param.Name)) {
                var query = players.AsQueryable();
                query = query.Where(players => (players.Lastname + players.Firstname).ToLower().Contains(param.Name.ToLower()));
                players = query.ToList();
            }

            if (players is not null) {
                var response = _mapper.Map<List<PlayerGetAllResponseForHirer>>(players);
                return PagedResult<PlayerGetAllResponseForHirer>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
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

            _context.Entry(player)
                .Collection(p => p.Images)
                .Query()
                .OrderByDescending(i => i.CreatedDate)
                .Load();
            return _mapper.Map<PlayerGetByIdResponseForPlayer>(player);
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

        public async Task<bool> UpdatePlayerServiceInfoAsync(string id, PlayerServiceInfoUpdateRequest request)
        {
            var player = await _context.Players.FindAsync(id);
            if (player is null) {
                return false;
            }

            if (request.Status) {
                player.Status = PlayerStatusConstants.Ready;
            }
            else {
                player.Status = PlayerStatusConstants.Offline;
            }
            player.MaxHourHire = request.MaxHourHire;
            player.PricePerHour = request.PricePerHour;

            _context.Players.Update(player);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
