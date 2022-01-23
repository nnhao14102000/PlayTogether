using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Player
{
    public interface IPlayerRepository
    {
        Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(PlayerParameters param);
        Task<PlayerProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdAsync(string id);
        Task<bool> UpdatePlayerInformationAsync(string id, PlayerUpdateInfoRequest request);
    }
}
