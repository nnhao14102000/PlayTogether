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

namespace PlayTogether.Infrastructure.Repositories.Business.GameType
{
    public class GameTypeRepository : BaseRepository, IGameTypeRepository
    {
        public GameTypeRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<GameTypeCreateResponse> CreateGameTypeAsync(GameTypeCreateRequest request)
        {
            var model = _mapper.Map<Entities.GameType>(request);
            await _context.GameTypes.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<GameTypeCreateResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteGameTypeAsync(string gameTypeId)
        {
            var type = await _context.GameTypes.FindAsync(gameTypeId);
            if (type is null) {
                return false;
            }

            var typeOfGames = await _context.TypeOfGames.Where(x => x.GameTypeId == gameTypeId).ToListAsync();
            if (typeOfGames.Count > 0) {
                _context.TypeOfGames.RemoveRange(typeOfGames);
                if ((await _context.SaveChangesAsync() < 0)) {
                    return false;
                }
            }
            
            _context.GameTypes.Remove(type);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param)
        {
            var types = await _context.GameTypes.ToListAsync();

            if (types is not null) {
                if (!String.IsNullOrEmpty(param.SearchString)) {
                    var query = types.AsQueryable();
                    query = query.Where(x => (x.Name + " " + x.ShortName + " " + x.OtherName + " " + x.Description).ToLower()
                                                       .Contains(param.SearchString.ToLower()));
                    types = query.ToList();
                }
                var response = _mapper.Map<List<GameTypeGetAllResponse>>(types);
                return PagedResult<GameTypeGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
        }

        public async Task<GameTypeGetByIdResponse> GetGameTypeByIdAsync(string gameTypeId)
        {
            var type = await _context.GameTypes.FindAsync(gameTypeId);

            if (type is null) {
                return null;
            }

            await _context.Entry(type)
                .Collection(t => t.TypeOfGames)
                .Query()
                .Include(tog => tog.Game)
                .LoadAsync();

            return _mapper.Map<GameTypeGetByIdResponse>(type);
        }

        public async Task<bool> UpdateGameTypeAsync(string id, GameTypeUpdateRequest request)
        {
            var type = await _context.GameTypes.FindAsync(id);

            if (type is null) {
                return false;
            }

            var model = _mapper.Map(request, type);
            _context.GameTypes.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}