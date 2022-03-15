using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IGameTypeService
    {
        Task<GameTypeCreateResponse> CreateGameTypeAsync(GameTypeCreateRequest request);
        Task<PagedResult<GameTypeGetAllResponse>> GetAllGameTypesAsync(GameTypeParameter param);
        Task<GameTypeGetByIdResponse> GetGameTypeByIdAsync(string gameTypeId);
        Task<bool> UpdateGameTypeAsync(string gameTypeId, GameTypeUpdateRequest request);
        Task<bool> DeleteGameTypeAsync(string gameTypeId);
    }
}