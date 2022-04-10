using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IGameTypeRepository
    {
        Task<Result<GameTypeCreateResponse>> CreateGameTypeAsync(GameTypeCreateRequest request);
        Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param);
        Task<Result<GameTypeGetByIdResponse>> GetGameTypeByIdAsync(string gameTypeId);
        Task<Result<bool>> UpdateGameTypeAsync(string gameTypeId, GameTypeUpdateRequest request);
        Task<Result<bool>> DeleteGameTypeAsync(string gameTypeId);
    }
}