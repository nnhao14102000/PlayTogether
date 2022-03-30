using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface ICharityService
    {
        Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param);
        Task<CharityResponse> GetCharityByIdAsync(string id);
        Task<bool> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request);
        Task<CharityResponse> GetProfileAsync(ClaimsPrincipal principal);
    }
}
