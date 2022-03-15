using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IRankService
    {
        Task<IEnumerable<RankGetAllResponse>> GetAllRanksInGameAsync(string gameId);

        Task<RankCreateResponse> CreateRankAsync(string gameId, RankCreateRequest request);
        Task<RankGetByIdResponse> GetRankByIdAsync(string rankId);
        Task<bool> UpdateRankAsync(string rankId, RankUpdateRequest request);
        Task<bool> DeleteRankAsync(string rankId);
    }
}