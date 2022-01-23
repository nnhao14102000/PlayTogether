using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Hirer
{
    public interface IHirerRepository
    {
        Task<PagedResult<GetAllHirerResponseForAdmin>> GetAllHirersForAdminAsync(HirerParameters param);
        Task<GetHirerProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<GetHirerByIdResponseForHirer> GetHirerByIdForHirerAsync(string id);
        Task<bool> UpdateHirerInformationAsync(string id, UpdateHirerInfoRequest request);
    }
}
