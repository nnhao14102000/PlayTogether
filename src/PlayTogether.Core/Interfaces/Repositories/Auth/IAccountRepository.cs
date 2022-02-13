using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Auth
{
    public interface IAccountRepository
    {
        Task<bool> CheckExistEmailAsync(string email);
        Task<AuthResult> LoginHirerByGoogleAsync(GoogleLoginRequest loginEmailDto);
        Task<AuthResult> LoginPlayerByGoogleAsync(GoogleLoginRequest loginEmailDto);
        Task<AuthResult> LoginUserAsync(LoginRequest loginDto);
        Task<AuthResult> RegisterAdminAsync(RegisterAdminInfoRequest registerDto);
        Task<AuthResult> RegisterCharityAsync(RegisterCharityInfoRequest registerDto);
        Task<AuthResult> RegisterHirerAsync(RegisterUserInfoRequest registerDto);
        Task<AuthResult> RegisterPlayerAsync(RegisterUserInfoRequest registerDto);
    }
}