using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IRankRepository
    {
        Task<PagedResult<RankGetAllResponse>> GetAllRanksInGameAsync(string gameId, RankParameters param);
        Task<Result<RankCreateResponse>> CreateRankAsync(string gameId, RankCreateRequest request);
        Task<Result<RankGetByIdResponse>> GetRankByIdAsync(string rankId);
        Task<Result<bool>> UpdateRankAsync(string rankId, RankUpdateRequest request);
        Task<Result<bool>> DeleteRankAsync(string rankId);
    }
}