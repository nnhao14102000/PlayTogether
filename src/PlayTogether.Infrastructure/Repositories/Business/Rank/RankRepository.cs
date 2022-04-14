using System.Linq;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using PlayTogether.Core.Dtos.Incoming.Generic;

namespace PlayTogether.Infrastructure.Repositories.Business.Rank
{
    public class RankRepository : BaseRepository, IRankRepository
    {
        public RankRepository(
            IMapper mapper,
            AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<RankCreateResponse>> CreateRankAsync(string gameId, RankCreateRequest request)
        {
            var result = new Result<RankCreateResponse>();
            var game = await _context.Games.FindAsync(gameId);
            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game.");
                return result;
            }

            var existNO = await _context.Ranks.Where(x => x.GameId == gameId)
                                              .AnyAsync(x => x.NO == request.NO);
            if (existNO) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" rank vs NO là {request.NO}. Vui lòng kiểm tra và thử lại.");
                return result;
            }

            var existName = await _context.Ranks.Where(x => x.GameId == gameId)
                                                .AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (existName) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" rank vs Name là {request.Name}. Vui lòng kiểm tra và thử lại.");
                return result;
            }

            var model = _mapper.Map<Core.Entities.Rank>(request);
            model.GameId = gameId;
            await _context.Ranks.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                var response = _mapper.Map<RankCreateResponse>(model);
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteRankAsync(string rankId)
        {
            var result = new Result<bool>();
            var rank = await _context.Ranks.FindAsync(rankId);
            if (rank is null) {
                if (rank is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" rank.");
                    return result;
                }
            }
            _context.Ranks.Remove(rank);

            if (await _context.SaveChangesAsync() >= 0) {
                var rankInGameOfUsers = await _context.GameOfUsers.Where(x => x.RankId == rank.Id).ToListAsync();
                foreach (var gameOfUser in rankInGameOfUsers) {
                    gameOfUser.RankId = "";
                }
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<RankGetAllResponse>> GetAllRanksInGameAsync(string gameId, RankParameters param)
        {
            var result = new PagedResult<RankGetAllResponse>();
            var game = await _context.Games.FindAsync(gameId);
            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game.");
                return result;
            }
            var ranksInGame = await _context.Ranks.Where(x => x.GameId == gameId)
                                                  .OrderBy(x => x.NO)
                                                  .ToListAsync();
            var response = _mapper.Map<List<RankGetAllResponse>>(ranksInGame);

            return PagedResult<RankGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<Result<RankGetByIdResponse>> GetRankByIdAsync(string rankId)
        {
            var result = new Result<RankGetByIdResponse>();
            var rank = await _context.Ranks.FindAsync(rankId);

            if (rank is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" rank.");
                return result;
            }

            var response = _mapper.Map<RankGetByIdResponse>(rank);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateRankAsync(string rankId, RankUpdateRequest request)
        {
            var result = new Result<bool>();
            var rank = await _context.Ranks.FindAsync(rankId);

            if (rank is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" rank.");
                return result;
            }

            var existName = await _context.Ranks.Where(x => x.Id != rankId)
                                                .AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (existName) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" rank vs Name là {request.Name}. Vui lòng kiểm tra và thử lại.");
                return result;
            }

            var model = _mapper.Map(request, rank);
            _context.Ranks.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}