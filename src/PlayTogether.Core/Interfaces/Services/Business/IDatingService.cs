using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IDatingService
    {
        Task<bool> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request);
        Task<bool> DeleteDatingAsync(ClaimsPrincipal principal, string datingId);
    }
}