using PlayTogether.Core.Dtos.Incoming.Business.MusicOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.MusicOfPlayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IMusicOfPlayerRepository
    {
        Task<IEnumerable<MusicOfPlayerGetAllResponse>> GetALlMusicOfPlayerAsync(string playerId);
        Task<MusicOfPlayerGetByIdResponse> CreateMusicOfPlayerAsync(string playerId, MusicOfPlayerCreateRequest request);
        Task<MusicOfPlayerGetByIdResponse> GetMusicOfPlayerByIdAsync(string id);
        Task<bool> DeleteMusicOfPlayerAsync(string id);
    }
}