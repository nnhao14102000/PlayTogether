using System.Collections.Generic;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IRankRepository
    {
        Task<IEnumerable<RankGetByIdResponse>> GetAllRanksInGameAsync(string gameId);

        Task<RankCreateResponse> CreateRankAsync(string gameId, RankCreateRequest request);
        Task<RankGetByIdResponse> GetRankByIdAsync(string id);
        Task<bool> UpdateRankAsync(string id, RankUpdateRequest request);
        Task<bool> DeleteRankAsync(string id);
    }
}