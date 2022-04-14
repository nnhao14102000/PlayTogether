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
        Task<Result<CharityResponse>> GetCharityByIdAsync(string charityId);
        Task<Result<bool>> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request);
        Task<Result<CharityResponse>> GetProfileAsync(ClaimsPrincipal principal);
        Task<Result<bool>> UpdateProfileAsync(ClaimsPrincipal principal, string charityId, CharityUpdateRequest request);
        Task<Result<bool>> CharityWithDrawAsync(ClaimsPrincipal principal, CharityWithDrawRequest request);
    }
}
