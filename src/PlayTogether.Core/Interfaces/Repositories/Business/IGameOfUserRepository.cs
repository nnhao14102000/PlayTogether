using System.Security.Claims;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IGameOfUserRepository
    {
        Task<PagedResult<GamesOfUserResponse>> GetAllGameOfUserAsync(string userId, GameOfUserParameters param);
        Task<Result<GameOfUserGetByIdResponse>> CreateGameOfUserAsync(ClaimsPrincipal principal, GameOfUserCreateRequest request);
        Task<Result<GameOfUserGetByIdResponse>> GetGameOfUserByIdAsync(string gameOfUserId);
        Task<Result<bool>> UpdateGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId, GameOfUserUpdateRequest request);
        Task<Result<bool>> DeleteGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId);

    }
}