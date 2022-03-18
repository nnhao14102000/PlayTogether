using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IGameOfUserService
    {
        Task<IEnumerable<GamesOfUserResponse>> GetAllGameOfUserAsync(string userId);

        Task<GameOfUserGetByIdResponse> CreateGameOfUserAsync(ClaimsPrincipal principal, GameOfUserCreateRequest request);

        Task<GameOfUserGetByIdResponse> GetGameOfUserByIdAsync(string gameOfUserId);

        Task<bool> UpdateGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId, GameOfUserUpdateRequest request);

        Task<bool> DeleteGameOfUserAsync(ClaimsPrincipal principal, string gameOfUserId);
    }
}