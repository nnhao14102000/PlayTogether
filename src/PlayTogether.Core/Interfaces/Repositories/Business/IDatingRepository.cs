using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IDatingRepository
    {
        Task<bool> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request);
        Task<bool> DeleteDatingAsync(ClaimsPrincipal principal, string datingId);
    }
}