using PlayTogether.Core.Dtos.Incoming.Business.Player;
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
        Task<PagedResult<GetAllPlayerResponseForHirer>> GetAllPlayersForHirerAsync(PlayerParameters param);
        Task<GetPlayerProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<GetPlayerByIdResponseForPlayer> GetPlayerByIdForPlayerAsync(string id);
        Task<bool> UpdatePlayerInformationAsync(string id, UpdatePlayerInfoRequest request);
    }
}
