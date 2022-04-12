using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IGameOfUserService
    {
        Task<PagedResult<GamesOfUserResponse>> GetAllGameOfUserAsync(string userId, GameOfUserParameters param);
        Task<Result<GameOfUserGetByIdResponse>> CreateGameOfUserAsync(ClaimsPrincipal principal, GameOfUserCreateRequest request);
        Task<Result<bool>> CreateMultiGameOfUserAsync(ClaimsPrincipal principal, List<GameOfUserCreateRequest> requests);
        Task<Result<GameOfUserGetByIdResponse>> GetGameOfUserByIdAsync(string gameOfUserId);
        Task<Result<bool>> UpdateGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId, GameOfUserUpdateRequest request);
        Task<Result<bool>> DeleteGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId);
    }
}