using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IAppUserService
    {
        Task<PersonalInfoResponse> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal);
        Task<bool> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request);
        Task<bool> UpdateUserServiceInfoAsync(ClaimsPrincipal principal, UserInfoForIsPlayerUpdateRequest request);
        Task<bool> ChangeIsPlayerAsync(ClaimsPrincipal principal, UserIsPlayerChangeRequest request);
        Task<UserGetServiceInfoResponse> GetUserServiceInfoByIdAsync(string userId);
        
        Task<UserGetBasicInfoResponse> GetUserBasicInfoByIdAsync(string userId);
        Task<PagedResult<UserSearchResponse>> GetAllUsersAsync(ClaimsPrincipal principal, UserParameters param);
        Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param);
        Task<bool> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request);
        Task<DisableUserResponse> GetDisableInfoAsync(ClaimsPrincipal principal);
        Task<bool> ActiveUserAsync(ClaimsPrincipal principal);
    }
}