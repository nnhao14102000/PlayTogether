using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginUserAsync(LoginDto loginDto);
        Task<AuthResultDto> LoginHirerByGoogle(GoogleLoginDto loginEmailDto);
        Task<AuthResultDto> LoginPlayerByGoogle(GoogleLoginDto loginEmailDto);
        Task<AuthResultDto> RegisterAdminAsync(RegisterDto registerDto);
        Task<AuthResultDto> RegisterCharityAsync(RegisterDto registerDto);
        Task<AuthResultDto> RegisterPlayerAsync(RegisterDto registerDto);
        Task<AuthResultDto> RegisterHirerAsync(RegisterDto registerDto);
    }
}