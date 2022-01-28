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
    public class GameTypeRepository : IGameTypeRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public GameTypeRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<GameTypeCreateResponse> CreateGameTypeAsync(GameTypeCreateRequest request)
        {
            var model = _mapper.Map<Entities.GameType>(request);
            _context.GameTypes.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<GameTypeCreateResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteGameTypeAsync(string id)
        {
            var type = await _context.GameTypes.FindAsync(id);
            if (type is null) {
                return false;
            }
            _context.GameTypes.Remove(type);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param)
        {
            var types = await _context.GameTypes.ToListAsync();

            if (types is not null) {
                if (!String.IsNullOrEmpty(param.Name)) {
                    var query = types.AsQueryable();
                    query = query.Where(x => x.TypeName.ToLower().Contains(param.Name.ToLower()));
                    types = query.ToList();
                }
                var response = _mapper.Map<List<GameTypeGetAllResponse>>(types);
                return PagedResult<GameTypeGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
        }

        public async Task<GameTypeGetByIdResponse> GetGameTypeByIdAsync(string id)
        {
            var type = await _context.GameTypes.FindAsync(id);

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