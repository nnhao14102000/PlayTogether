using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<bool> CheckExistEmailAsync(string email);
        Task<AuthResultDto> LoginHirerByGoogleAsync(GoogleLoginRequest loginEmailDto);
        Task<AuthResultDto> LoginPlayerByGoogleAsync(GoogleLoginRequest loginEmailDto);
        Task<AuthResultDto> LoginUserAsync(LoginRequest loginDto);
        Task<AuthResultDto> RegisterAdminAsync(RegisterAdminInfoRequest registerDto);
        Task<AuthResultDto> RegisterCharityAsync(RegisterCharityInfoRequest registerDto);
        Task<AuthResultDto> RegisterHirerAsync(RegisterUserInfoRequest registerDto);
        Task<AuthResultDto> RegisterPlayerAsync(RegisterUserInfoRequest registerDto);
    }
}