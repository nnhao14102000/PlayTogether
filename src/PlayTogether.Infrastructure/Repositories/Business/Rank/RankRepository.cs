using System.Linq;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PlayTogether.Infrastructure.Repositories.Business.Rank
{
    public class RankRepository : BaseRepository, IRankRepository
    {
        public RankRepository(
            IMapper mapper,
            AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<RankCreateResponse> CreateRankAsync(string gameId, RankCreateRequest request)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game is null) {
                return null;
            }

            var existNO = await _context.Ranks.Where(x => x.GameId == gameId)
                                              .AnyAsync(x => x.NO == request.NO);
            if (existNO) {
                return null;
            }
            
            var model = _mapper.Map<Entities.Rank>(request);
            model.GameId = gameId;
            await _context.Ranks.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<RankCreateResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteRankAsync(string id)
        {
            var rank = await _context.Ranks.FindAsync(id);
            if (rank is null) {
                return false;
            }
            _context.Ranks.Remove(rank);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<RankGetByIdResponse>> GetAllRanksInGameAsync(string gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game is null) {
                return null;
            }
            var ranksInGame = await _context.Ranks.Where(x => x.GameId == gameId)
                                                  .OrderBy(x => x.NO)
                                                  .ToListAsync();

            return _mapper.Map<IEnumerable<RankGetByIdResponse>>(ranksInGame);
        }

        public async Task<RankGetByIdResponse> GetRankByIdAsync(string id)
        {
            var rank = await _context.Ranks.FindAsync(id);

            if (rank is null) {
                return null;
            }

            return _mapper.Map<RankGetByIdResponse>(rank);
        }

        public async Task<bool> UpdateRankAsync(string id, RankUpdateRequest request)
        {
            var rank = await _context.Ranks.FindAsync(id);

            if (rank is null) {
                return false;
            }

            var model = _mapper.Map(request, rank);
            _context.Ranks.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}