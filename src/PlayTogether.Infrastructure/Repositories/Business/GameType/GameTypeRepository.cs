using System.Collections.Generic;
using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Threading.Tasks;
using System.Linq;
using PlayTogether.Core.Dtos.Incoming.Generic;

namespace PlayTogether.Infrastructure.Repositories.Business.GameType
{
    public class GameTypeRepository : BaseRepository, IGameTypeRepository
    {
        public GameTypeRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<GameTypeCreateResponse>> CreateGameTypeAsync(GameTypeCreateRequest request)
        {
            var result = new Result<GameTypeCreateResponse>();
            var existGameType = await _context.GameTypes.AnyAsync(x => x.Name == request.Name || x.ShortName == request.Name || x.OtherName == request.Name);
            if (existGameType) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" thể loại game {request.Name}");
                return result;
            }
            var model = _mapper.Map<Entities.GameType>(request);
            await _context.GameTypes.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                var response = _mapper.Map<GameTypeCreateResponse>(model);
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteGameTypeAsync(string gameTypeId)
        {
            var result = new Result<bool>();
            var type = await _context.GameTypes.FindAsync(gameTypeId);
            if (type is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thể loại {gameTypeId}");
                return result;
            }

            var typeOfGames = await _context.TypeOfGames.Where(x => x.GameTypeId == gameTypeId).ToListAsync();
            if (typeOfGames.Count > 0) {
                _context.TypeOfGames.RemoveRange(typeOfGames);
                if ((await _context.SaveChangesAsync() < 0)) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

            _context.GameTypes.Remove(type);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param)
        {
            var types = await _context.GameTypes.ToListAsync();
            var query = types.AsQueryable();
            FilterGameTypeByName(ref query, param.Name);
            types = query.ToList();
            var response = _mapper.Map<List<GameTypeGetAllResponse>>(types);
            return PagedResult<GameTypeGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);


        }

        private void FilterGameTypeByName(ref IQueryable<Entities.GameType> query, string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => (x.Name + " " + x.ShortName + " " + x.OtherName + " " + x.Description).ToLower()
                                                   .Contains(name.ToLower()));
        }

        public async Task<Result<GameTypeGetByIdResponse>> GetGameTypeByIdAsync(string gameTypeId)
        {
            var result = new Result<GameTypeGetByIdResponse>();
            var type = await _context.GameTypes.FindAsync(gameTypeId);

            if (type is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thể loại {gameTypeId}");
                return result;
            }

            await _context.Entry(type)
                .Collection(t => t.TypeOfGames)
                .Query()
                .Include(tog => tog.Game)
                .LoadAsync();

            var response = _mapper.Map<GameTypeGetByIdResponse>(type);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateGameTypeAsync(string gameTypeId, GameTypeUpdateRequest request)
        {
            var result = new Result<bool>();
            var type = await _context.GameTypes.FindAsync(gameTypeId);

            if (type is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thể loại {gameTypeId}");
                return result;
            }

            var existGameType = await _context.GameTypes.Where(x => x.Id != gameTypeId).AnyAsync(x => x.Name == request.Name || x.ShortName == request.Name || x.OtherName == request.Name);
            if (existGameType) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" thể loại game {request.Name}");
                return result;
            }

            var model = _mapper.Map(request, type);
            _context.GameTypes.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}