using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IGameService
    {
        Task<GameCreateResponse> CreateGameAsync(GameCreateRequest request);
        Task<PagedResult<GameGetAllResponse>> GetAllGamesAsync(GameParameter param);
        Task<GameGetByIdResponse> GetGameByIdAsync(string gameId);
        Task<bool> UpdateGameAsync(string gameId, GameUpdateRequest request);
        Task<bool> DeleteGameAsync(string gameId);
    }
}