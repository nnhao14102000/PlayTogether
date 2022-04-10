using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IGameRepository
    {
        Task<Result<GameCreateResponse>> CreateGameAsync(GameCreateRequest request);
        Task<PagedResult<GameGetAllResponse>> GetAllGamesAsync(GameParameters param);
        Task<Result<GameGetByIdResponse>> GetGameByIdAsync(string gameId);
        Task<Result<bool>> UpdateGameAsync(string gameId, GameUpdateRequest request);
        Task<Result<bool>> DeleteGameAsync(string gameId);
    }
}