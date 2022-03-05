using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IHirerRepository
    {
        Task<PagedResult<HirerGetAllResponseForAdmin>> GetAllHirersForAdminAsync(HirerParameters param);
        Task<bool> UpdateHirerStatusForAdminAsync(string hirerId, HirerStatusUpdateRequest request);
        
        Task<HirerGetProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<HirerGetByIdResponse> GetHirerByIdAsync(string hirerId);
        Task<bool> UpdateHirerInformationAsync(string hirerId, HirerInfoUpdateRequest request);
    }
}
