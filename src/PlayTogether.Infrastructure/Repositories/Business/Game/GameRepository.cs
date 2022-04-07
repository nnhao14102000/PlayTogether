using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Game
{
    public class GameRepository : BaseRepository, IGameRepository
    {
        public GameRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<GameCreateResponse> CreateGameAsync(GameCreateRequest request)
        {
            var exitGame = await _context.Games.AnyAsync(x => (x.Name + x.OtherName + x.DisplayName).ToLower().Contains(request.Name));
            var model = _mapper.Map<Entities.Game>(request);
            await _context.Games.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<GameCreateResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteGameAsync(string gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game is null) {
                return false;
            }

            var ranks = await _context.Ranks.Where(x => x.GameId == gameId).ToListAsync();
            if (ranks.Count >= 0) {
                _context.Ranks.RemoveRange(ranks);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            var gameOfUser = await _context.GameOfUsers.Where(x => x.GameId == gameId).ToListAsync();
            if (gameOfUser.Count >= 0) {
                _context.GameOfUsers.RemoveRange(gameOfUser);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            var gameOfOrder = await _context.GameOfOrders.Where(x => x.GameId == gameId).ToListAsync();
            if (gameOfOrder.Count >= 0) {
                _context.GameOfOrders.RemoveRange(gameOfOrder);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            var hobbies = await _context.Hobbies.Where(x => x.GameId == gameId).ToListAsync();
            if (hobbies.Count >= 0) {
                _context.Hobbies.RemoveRange(hobbies);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            var typeOfGame = await _context.TypeOfGames.Where(x => x.GameId == gameId).ToListAsync();
            if (typeOfGame.Count >= 0) {
                _context.TypeOfGames.RemoveRange(typeOfGame);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            _context.Games.Remove(game);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<GameGetAllResponse>> GetAllGamesAsync(GameParameter param)
        {
            var games = await _context.Games.ToListAsync();
            var query = games.AsQueryable();

            OrderByMostFavorite(ref query, param.IsMostFavorite);
            FilterByName(ref query, param.Name);
            OrderByCreatedDate(ref query, param.IsNew);

            games = query.ToList();
            var response = _mapper.Map<List<GameGetAllResponse>>(games);
            return PagedResult<GameGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);

        }

        private void OrderByMostFavorite(ref IQueryable<Entities.Game> query, bool? isMostFavorite)
        {
            if(!query.Any() || isMostFavorite is null || isMostFavorite is false){
                return;
            }
            var listGameOfUser = _context.GameOfUsers.GroupBy(x => x.GameId).Select(g => new {gameId = g.Key, count = g.Count()}).OrderByDescending(x => x.count);

            var listGame = new List<Entities.Game>();
            foreach (var gameOfUser in listGameOfUser)
            {
                var game = _context.Games.Find(gameOfUser.gameId);
                if (game is null) continue;
                listGame.Add(game);
            }
            query = listGame.AsQueryable();
        }

        private void OrderByCreatedDate(ref IQueryable<Entities.Game> query, bool? isNew)
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

        private void FilterByName(ref IQueryable<Entities.Game> query, string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => (x.Name + " " + x.DisplayName + " " + x.OtherName).ToLower()
                                                   .Contains(name.ToLower()));
        }

        public async Task<GameGetByIdResponse> GetGameByIdAsync(string gameId)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game is null) {
                return null;
            }

            await _context.Entry(game)
                .Collection(g => g.TypeOfGames)
                .Query()
                .Include(tog => tog.GameType)
                .LoadAsync();

            await _context.Entry(game)
                .Collection(g => g.Ranks)
                .Query()
                .OrderBy(r => r.NO)
                .LoadAsync();

            return _mapper.Map<GameGetByIdResponse>(game);
        }

        public async Task<bool> UpdateGameAsync(string gameId, GameUpdateRequest request)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game is null) {
                return false;
            }

            var model = _mapper.Map(request, game);
            _context.Games.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}