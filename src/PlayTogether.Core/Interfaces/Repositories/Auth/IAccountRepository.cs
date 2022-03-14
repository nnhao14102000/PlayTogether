using System.Security.Claims;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Auth
{
    public interface IAccountRepository
    {
        Task<bool> CheckExistEmailAsync(string email);
        Task<AuthResult> LoginUserByGoogleAsync(GoogleLoginRequest request);
        Task<AuthResult> LoginUserAsync(LoginRequest request);
        Task<AuthResult> LoginCharityAsync(LoginRequest request);
        Task<AuthResult> LoginAdminAsync(LoginRequest request);
        Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest request);
        Task<AuthResult> RegisterCharityAsync(RegisterCharityInfoRequest request);
        Task<AuthResult> RegisterUserAsync(RegisterUserInfoRequest request);
        Task<AuthResult> LogoutAsync(ClaimsPrincipal principal);
        Task<AuthResult> ChangePasswordAsync(ChangePasswordRequest request);
        Task<AuthResult> ResetPasswordAdminAsync(ResetPasswordAdminRequest request);
        Task<AuthResult> ResetPasswordTokenAsync(ResetPasswordTokenRequest request);
        Task<AuthResult> ResetPasswordAsync(ResetPasswordRequest request);

        // Extra
        Task<bool> RegisterMultiUserAsync(List<RegisterUserInfoRequest> request);
        Task<bool> RegisterMultiUserIsPlayerAsync(List<RegisterUserInfoRequest> request);
    }
}