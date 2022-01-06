using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<AuthResultDto> LoginUserAsync(LoginDto loginDto);
        Task<AuthResultDto> RegisterAdminAsync(RegisterDto registerDto);
    }
}