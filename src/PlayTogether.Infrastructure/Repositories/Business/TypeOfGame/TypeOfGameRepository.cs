using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.TypeOfGame
{
    public class TypeOfGameRepository : ITypeOfGameRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public TypeOfGameRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<TypeOfGameGetByIdResponse> CreateTypeOfGameAsync(TypeOfGameCreateRequest request)
        {
            var model = _mapper.Map<Entities.TypeOfGame>(request);
            await _context.TypeOfGames.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<TypeOfGameGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteTypeOfGameAsync(string id)
        {
            var model = await _context.TypeOfGames.FindAsync(id);
            if (model is null) {
                return false;
            }
            _context.TypeOfGames.Remove(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<TypeOfGameGetByIdResponse> GetTypeOfGameByIdAsync(string id)
        {
            var model = await _context.TypeOfGames.FindAsync(id);
            if (model is null) {
                return null;
            }
            return _mapper.Map<TypeOfGameGetByIdResponse>(model);
        }
    }
}