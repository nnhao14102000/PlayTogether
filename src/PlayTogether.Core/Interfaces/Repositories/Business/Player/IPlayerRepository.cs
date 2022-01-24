﻿using PlayTogether.Core.Dtos.Incoming.Business.Player;
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
        Task<PlayerGetProfileResponse> GetPlayerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<PlayerGetByIdResponseForPlayer> GetPlayerByIdForPlayerAsync(string id);
        Task<bool> UpdatePlayerInformationAsync(string id, PlayerInfoUpdateRequest request);
    }
}
