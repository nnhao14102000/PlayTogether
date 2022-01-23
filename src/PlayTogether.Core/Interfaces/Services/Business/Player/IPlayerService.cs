using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Player
{
    public interface IPlayerService
    {
        Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(PlayerParameters param);
        Task<PlayerProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdAsync(string id);
        Task<bool> UpdatePlayerInformationAsync(string id, PlayerUpdateInfoRequest request);
    }
}
