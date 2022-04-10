using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Incoming.Generic;
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

        public async Task<Result<GameCreateResponse>> CreateGameAsync(GameCreateRequest request)
        {
            var result = new Result<GameCreateResponse>();
            var exitGame = await _context.Games.AnyAsync(x => (x.Name).ToLower() == request.Name.ToLower());
            if (exitGame is true) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" game có tên {request.Name}");
                return result;
            }

            var model = _mapper.Map<Entities.Game>(request);
            await _context.Games.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                var response = _mapper.Map<GameCreateResponse>(model);
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<BooleanContent>> DeleteGameAsync(string gameId)
        {
            var result = new Result<BooleanContent>();
            var game = await _context.Games.FindAsync(gameId);
            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game {gameId}");
                return result;
            }

            var ranks = await _context.Ranks.Where(x => x.GameId == gameId).ToListAsync();
            if (ranks.Count >= 0) {
                _context.Ranks.RemoveRange(ranks);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            var gameOfUser = await _context.GameOfUsers.Where(x => x.GameId == gameId).ToListAsync();
            if (gameOfUser.Count >= 0) {
                _context.GameOfUsers.RemoveRange(gameOfUser);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            var gameOfOrder = await _context.GameOfOrders.Where(x => x.GameId == gameId).ToListAsync();
            if (gameOfOrder.Count >= 0) {
                _context.GameOfOrders.RemoveRange(gameOfOrder);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            var hobbies = await _context.Hobbies.Where(x => x.GameId == gameId).ToListAsync();
            if (hobbies.Count >= 0) {
                _context.Hobbies.RemoveRange(hobbies);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            var typeOfGame = await _context.TypeOfGames.Where(x => x.GameId == gameId).ToListAsync();
            if (typeOfGame.Count >= 0) {
                _context.TypeOfGames.RemoveRange(typeOfGame);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            _context.Games.Remove(game);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = new BooleanContent(SuccessMessageConstants.Delete);
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
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
            if (!query.Any() || isMostFavorite is null || isMostFavorite is false) {
                return;
            }
            var listGameFavorite = _context.GameOfUsers.GroupBy(x => x.GameId).Select(g => new { gameId = g.Key, count = g.Count() }).Union(_context.Hobbies.GroupBy(x => x.GameId).Select(g => new { gameId = g.Key, count = g.Count() })).OrderByDescending(x => x.count);

            var listGame = new List<Entities.Game>();
            foreach (var gameOfUser in listGameFavorite) {
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

        public async Task<Result<GameGetByIdResponse>> GetGameByIdAsync(string gameId)
        {
            var result = new Result<GameGetByIdResponse>();
            var game = await _context.Games.FindAsync(gameId);

            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game {gameId}");
                return result;
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

            var response = _mapper.Map<GameGetByIdResponse>(game);
            result.Content = response;
            return result;
        }

        public async Task<Result<BooleanContent>> UpdateGameAsync(string gameId, GameUpdateRequest request)
        {
            var result = new Result<BooleanContent>();
            var game = await _context.Games.FindAsync(gameId);

            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game {gameId}");
                return result;
            }

            var exitGame = await _context.Games.AnyAsync(x => (x.Name).ToLower() == request.Name.ToLower() && x.Id != gameId);
            if (exitGame is true) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" game có tên {request.Name}");
                return result;
            }

            var model = _mapper.Map(request, game);
            _context.Games.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = new BooleanContent(SuccessMessageConstants.Update);
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}