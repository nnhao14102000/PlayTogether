using System.Collections.Generic;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfPlayer;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;

namespace PlayTogether.Infrastructure.Repositories.Business.GameOfPlayer
{
    public class GameOfPlayerRepository : BaseRepository, IGameOfPlayerRepository
    {
        public GameOfPlayerRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<IEnumerable<GamesInPlayerGetAllResponse>> GetAllGameOfPlayerAsync(string playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player is null) {
                return null;
            }

            var gamesOfPlayer = await _context.GameOfPlayers.Where(x => x.PlayerId == playerId)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .ToListAsync();

            foreach (var item in gamesOfPlayer) {
                await _context.Entry(item)
                              .Reference(x => x.Game)
                              .Query()
                              .LoadAsync();
            }

            return _mapper.Map<IEnumerable<GamesInPlayerGetAllResponse>>(gamesOfPlayer);

        }

        public async Task<GameOfPlayerGetByIdResponse> CreateGameOfPlayerAsync(string playerId, GameOfPlayerCreateRequest request)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player is null) {
                return null;
            }

            var game = await _context.Games.FindAsync(request.GameId);
            if (game is null) {
                return null;
            }

            var existGame = await _context.GameOfPlayers.Where(x => x.PlayerId == playerId).AnyAsync(x => x.GameId == request.GameId);
            if (existGame) {
                return null;
            }

            var model = _mapper.Map<Entities.GameOfPlayer>(request);
            model.PlayerId = playerId;
            await _context.GameOfPlayers.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<GameOfPlayerGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteGameOfPlayerAsync(string id)
        {
            var gameOfPlayer = await _context.GameOfPlayers.FindAsync(id);
            if (gameOfPlayer is null) {
                return false;
            }
            _context.GameOfPlayers.Remove(gameOfPlayer);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<GameOfPlayerGetByIdResponse> GetGameOfPlayerByIdAsync(string id)
        {
            var gameOfPlayer = await _context.GameOfPlayers.FindAsync(id);
            if (gameOfPlayer is null) {
                return null;
            }

            await _context.Entry(gameOfPlayer)
                              .Reference(x => x.Game)
                              .Query()
                              .LoadAsync();

            return _mapper.Map<GameOfPlayerGetByIdResponse>(gameOfPlayer);
        }

        public async Task<bool> UpdateGameOfPlayerAsync(string id, GameOfPlayerUpdateRequest request)
        {
            var gameOfPlayer = await _context.GameOfPlayers.FindAsync(id);
            if (gameOfPlayer is null) {
                return false;
            }

            var model = _mapper.Map(request, gameOfPlayer);
            _context.GameOfPlayers.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}