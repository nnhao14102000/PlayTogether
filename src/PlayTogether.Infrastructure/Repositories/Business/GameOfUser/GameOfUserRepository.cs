using System.Collections.Generic;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace PlayTogether.Infrastructure.Repositories.Business.GameOfUser
{
    public class GameOfUserRepository : BaseRepository, IGameOfUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public GameOfUserRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<GamesOfUserResponse>> GetAllGameOfUserAsync(string userId)
        {
            var player = await _context.AppUsers.FindAsync(userId);
            if (player is null) {
                return null;
            }

            var gamesOfPlayer = await _context.GameOfUsers.Where(x => x.UserId == userId)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .ToListAsync();

            foreach (var item in gamesOfPlayer) {
                await _context.Entry(item)
                              .Reference(x => x.Game)
                              .Query()
                              .LoadAsync();
            }

            return _mapper.Map<IEnumerable<GamesOfUserResponse>>(gamesOfPlayer);

        }

        public async Task<GameOfUserGetByIdResponse> CreateGameOfUserAsync(
            ClaimsPrincipal principal,
            GameOfUserCreateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return null;
            }

            var game = await _context.Games.FindAsync(request.GameId);
            if (game is null) {
                return null;
            }

            var existGame = await _context.GameOfUsers.Where(x => x.UserId == user.Id).AnyAsync(x => x.GameId == request.GameId);
            if (existGame) {
                return null;
            }

            var model = _mapper.Map<Entities.GameOfUser>(request);
            model.UserId = user.Id;
            if (String.IsNullOrEmpty(request.RankId) || String.IsNullOrWhiteSpace(request.RankId)) {
                model.RankId = "None";
            }else{
                var existRank = await _context.Ranks.Where(x => x.GameId == request.GameId).AnyAsync(x => x.Id == request.RankId);
                if(!existRank){
                    return null;
                }
            }
            await _context.GameOfUsers.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<GameOfUserGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteGameOfUserAsync(
            ClaimsPrincipal principal,
            string gameOfUserId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            var gameOfUser = await _context.GameOfUsers.FindAsync(gameOfUserId);
            if (gameOfUser is null || gameOfUser.UserId != user.Id) {
                return false;
            }
            _context.GameOfUsers.Remove(gameOfUser);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<GameOfUserGetByIdResponse> GetGameOfUserByIdAsync(string gameOfUserId)
        {
            var gameOfUser = await _context.GameOfUsers.FindAsync(gameOfUserId);
            if (gameOfUser is null) {
                return null;
            }

            await _context.Entry(gameOfUser)
                              .Reference(x => x.Game)
                              .Query()
                              .LoadAsync();

            return _mapper.Map<GameOfUserGetByIdResponse>(gameOfUser);
        }

        public async Task<bool> UpdateGameOfUserAsync(
            ClaimsPrincipal principal,
            string gameOfUserId,
            GameOfUserUpdateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            var gameOfUser = await _context.GameOfUsers.FindAsync(gameOfUserId);
            if (gameOfUser is null || gameOfUser.UserId != user.Id) {
                return false;
            }

            var model = _mapper.Map(request, gameOfUser);
            if (String.IsNullOrEmpty(request.RankId) || String.IsNullOrWhiteSpace(request.RankId)) {
                model.RankId = "None";
            }else{
                var existRank = await _context.Ranks.Where(x => x.GameId == gameOfUser.GameId).AnyAsync(x => x.Id == request.RankId);
                if(!existRank){
                    return false;
                }
            }
            _context.GameOfUsers.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}