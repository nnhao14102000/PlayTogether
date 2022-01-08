using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginHirerByGoogleAsync(GoogleLoginDto loginEmailDto);
        Task<AuthResultDto> LoginPlayerByGoogleAsync(GoogleLoginDto loginEmailDto);
        Task<AuthResultDto> LoginUserAsync(LoginDto loginDto);
        Task<AuthResultDto> RegisterAdminAsync(RegisterAdminInfoDto registerDto);
        Task<AuthResultDto> RegisterCharityAsync(RegisterCharityInfoDto registerDto);
        Task<AuthResultDto> RegisterHirerAsync(RegisterUserInfoDto registerDto);
        Task<AuthResultDto> RegisterPlayerAsync(RegisterUserInfoDto registerDto);
    }
}