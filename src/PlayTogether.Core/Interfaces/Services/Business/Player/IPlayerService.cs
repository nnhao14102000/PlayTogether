using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Player
{
    public interface IPlayerService
    {
        Task<PagedResult<PlayerResponse>> GetAllPlayersAsync(PlayerParameters param);
    }
}
