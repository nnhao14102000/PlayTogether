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
            var model = _mapper.Map<Entities.Game>(request);
            await _context.Games.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<GameCreateResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteGameAsync(string id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game is null) {
                return false;
            }

            var ranks = await _context.Ranks.Where(x => x.GameId == id).ToListAsync();
            if (ranks.Count >= 0) {
                _context.Ranks.RemoveRange(ranks);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            var gameOfPlayer = await _context.GameOfPlayers.Where(x => x.GameId == id).ToListAsync();
            if (gameOfPlayer.Count >= 0) {
                _context.GameOfPlayers.RemoveRange(gameOfPlayer);
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

            SearchByName(ref query, param.Name);

            games = query.ToList();
            var response = _mapper.Map<List<GameGetAllResponse>>(games);
            return PagedResult<GameGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);

        }

        private void SearchByName(ref IQueryable<Entities.Game> query, string name)
        {
            if(!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
            {
                return;
            }
            query = query.Where(x => (x.Name + " "  + x.DisplayName + " "  + x.OtherName).ToLower()
                                                   .Contains(name.ToLower()));
        }

        public async Task<GameGetByIdResponse> GetGameByIdAsync(string id)
        {
            var game = await _context.Games.FindAsync(id);

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

        public async Task<bool> UpdateGameAsync(string id, GameUpdateRequest request)
        {
            var game = await _context.Games.FindAsync(id);

            if (game is null) {
                return false;
            }

            var model = _mapper.Map(request, game);
            _context.Games.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}