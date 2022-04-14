using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Linq;
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

        public async Task<Result<bool>> CreateTypeOfGameAsync(TypeOfGameCreateRequest request)
        {
            var result = new Result<bool>();
            var game = await _context.Games.FindAsync(request.GameId);
            if (game is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game.");
                return result;
            }

            var gameType = await _context.GameTypes.FindAsync(request.GameTypeId);
            if (gameType is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thể loại game.");
                return result;
            }

            var typeOfGameExist = await _context.TypeOfGames.Where(x => x.GameId == request.GameId)
                                                            .AnyAsync(x => x.GameTypeId == request.GameTypeId);
            if(typeOfGameExist) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Exist + $" thể loại game.");
                return result;
            }
            var model = _mapper.Map<Core.Entities.TypeOfGame>(request);
            await _context.TypeOfGames.AddAsync(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteTypeOfGameAsync(string typeOfGameId)
        {
            var result = new Result<bool>();
            var typeOfGame = await _context.TypeOfGames.FindAsync(typeOfGameId);
            if (typeOfGame is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thể loại game.");
                return result;
            }
            _context.TypeOfGames.Remove(typeOfGame);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<TypeOfGameGetByIdResponse>> GetTypeOfGameByIdAsync(string typeOfGameId)
        {
            var result = new Result<TypeOfGameGetByIdResponse>();
            var typeOfGame = await _context.TypeOfGames.FindAsync(typeOfGameId);
            if (typeOfGame is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thể loại game.");
                return result;
            }
            var response = _mapper.Map<TypeOfGameGetByIdResponse>(typeOfGame);
            result.Content = response;
            return result;
        }
    }
}