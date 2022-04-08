using System.Security.Claims;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ICharityRepository
    {
        Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param);
        Task<CharityResponse> GetCharityByIdAsync(string charityId);
        Task<bool> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request);
        Task<CharityResponse> GetProfileAsync(ClaimsPrincipal principal);
        Task<bool> UpdateProfileAsync(ClaimsPrincipal principal, string charityId, CharityUpdateRequest request);
        Task<bool> CharityWithDrawAsync(ClaimsPrincipal principal, CharityWithDrawRequest request);
    }
}
