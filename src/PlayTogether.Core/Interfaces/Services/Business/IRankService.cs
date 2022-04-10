using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IRankService
    {
        Task<PagedResult<RankGetAllResponse>> GetAllRanksInGameAsync(string gameId, RankParameters param);
        Task<Result<RankCreateResponse>> CreateRankAsync(string gameId, RankCreateRequest request);
        Task<Result<RankGetByIdResponse>> GetRankByIdAsync(string rankId);
        Task<Result<bool>> UpdateRankAsync(string rankId, RankUpdateRequest request);
        Task<Result<bool>> DeleteRankAsync(string rankId);
    }
}