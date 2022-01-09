using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Player
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<PlayerResponseDto>> GetAllPlayerAsync();
    }
}
