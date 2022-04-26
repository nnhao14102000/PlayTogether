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
        Task<Result<PersonalInfoResponse>> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal);
        Task<Result<bool>> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request);
        Task<Result<bool>> UpdateUserServiceInfoAsync(ClaimsPrincipal principal, UserInfoForIsPlayerUpdateRequest request);
        Task<Result<bool>> ChangeIsPlayerAsync(ClaimsPrincipal principal, UserIsPlayerChangeRequest request);
        Task<Result<UserGetServiceInfoResponse>> GetUserServiceInfoByIdAsync(string userId);
        
        Task<Result<UserGetBasicInfoResponse>> GetUserBasicInfoByIdAsync(string userId);
        Task<PagedResult<UserSearchResponse>> GetAllUsersAsync(ClaimsPrincipal principal, UserParameters param);
        Task<Result<bool>> TurnOnIsPlayerWhenDatingComeOnAsync();
        Task<Result<bool>> UpdateRankingPointAsync();

        Task<PagedResult<UserGetByAdminResponse>> GetAllUsersForAdminAsync(AdminUserParameters param);
        Task<Result<bool>> ChangeIsActiveUserForAdminAsync(string userId, IsActiveChangeRequest request);
        Task<Result<DisableUserResponse>> GetDisableInfoAsync(ClaimsPrincipal principal);
        Task<Result<bool>> ActiveUserAsync(ClaimsPrincipal principal);
        Task<Result<BehaviorPointResponse>> GetBehaviorPointAsync(string userId);
        Task<Result<UserBalanceResponse>> GetBalanceAsync(string userId);
    }
}