using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IDatingService
    {
        Task<bool> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request);
        Task<bool> DeleteDatingAsync(ClaimsPrincipal principal, string datingId);
        Task<Result<DatingUserResponse>> GetDatingByIdAsync(string datingId);
    }
}