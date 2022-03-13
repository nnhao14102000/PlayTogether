using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IPlayerRepository
    {        
        // Task<PlayerGetProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        // Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdForPlayerAsync(string id);
        // Task<PlayerServiceInfoResponseForPlayer> GetPlayerServiceInfoByIdForPlayerAsync(string id);
        // Task<PlayerGetByIdResponseForHirer> GetPlayerByIdForHirerAsync(string id);
        // Task<bool> UpdatePlayerInformationAsync(string id, PlayerInfoUpdateRequest request);
        // Task<bool> UpdatePlayerServiceInfoAsync(string id, PlayerServiceInfoUpdateRequest request);
        
        // Task<PlayerOtherSkillResponse> GetPlayerOtherSkillByIdAsync(string id);
        // Task<bool> UpdatePlayerOtherSkillAsync(string id, OtherSkillUpdateRequest request);
        // Task<bool> AcceptPolicyAsync(ClaimsPrincipal principal, PlayerAcceptPolicyRequest request);

        // Task<PagedResult<PlayerGetAllResponseForHirer>> GetAllPlayersForHirerAsync(ClaimsPrincipal principal, PlayerParameters param);
        // Task<PagedResult<PlayerGetAllResponseForAdmin>> GetAllPlayersForAdminAsync(PlayerForAdminParameters param);
        // Task<PlayerGetByIdForAdminResponse> GetPlayerByIdForAdminAsync(string playerId);
        // Task<bool> UpdatePlayerStatusForAdminAsync (string playerId, PlayerStatusUpdateRequest request);

    }
}
