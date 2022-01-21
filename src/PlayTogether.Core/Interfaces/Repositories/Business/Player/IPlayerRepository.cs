using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Player
{
    public interface IPlayerRepository
    {
        Task<PagedResult<PlayerResponse>> GetAllPlayerAsync(PlayerParameters param);
    }
}
