using PlayTogether.Core.Dtos.Incoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IMusicRepository
    {
        Task<MusicGetByIdResponse> CreateMusicAsync(MusicCreateRequest request);
        Task<PagedResult<MusicGetByIdResponse>> GetAllMusicsAsync(MusicParameter param);
        Task<MusicGetByIdResponse> GetMusicByIdAsync(string id);
        Task<bool> UpdateMusicAsync(string id, MusicUpdateRequest request);
        Task<bool> DeleteMusicAsync(string id);
    }
}