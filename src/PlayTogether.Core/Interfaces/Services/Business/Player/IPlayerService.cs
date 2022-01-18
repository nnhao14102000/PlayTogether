using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Player
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerResponse>> GetAllPlayerAsync();
    }
}
