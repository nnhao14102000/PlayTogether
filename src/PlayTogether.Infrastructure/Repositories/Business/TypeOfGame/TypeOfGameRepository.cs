using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.TypeOfGame
{
    public class TypeOfGameRepository : BaseRepository, ITypeOfGameRepository
    {
        public TypeOfGameRepository(
            IMapper mapper,
            AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<bool> CreateTypeOfGameAsync(TypeOfGameCreateRequest request)
        {
            var game = await _context.Games.FindAsync(request.GameId);
            if (game is null) {
                return false;
            }

            var gameType = await _context.GameTypes.FindAsync(request.GameTypeId);
            if (gameType is null) {
                return false;
            }

            var model = _mapper.Map<Entities.TypeOfGame>(request);
            await _context.TypeOfGames.AddAsync(model);
            
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeleteTypeOfGameAsync(string typeOfGameId)
        {
            var model = await _context.TypeOfGames.FindAsync(typeOfGameId);
            if (model is null) {
                return false;
            }
            _context.TypeOfGames.Remove(model);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<TypeOfGameGetByIdResponse> GetTypeOfGameByIdAsync(string typeOfGameId)
        {
            var model = await _context.TypeOfGames.FindAsync(typeOfGameId);
            if (model is null) {
                return null;
            }
            return _mapper.Map<TypeOfGameGetByIdResponse>(model);
        }
    }
}