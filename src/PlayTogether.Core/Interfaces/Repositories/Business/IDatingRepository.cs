using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IDatingRepository
    {
        Task<Result<bool>> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request);
        Task<Result<bool>> DeleteDatingAsync(ClaimsPrincipal principal, string datingId);
        Task<Result<DatingUserResponse>> GetDatingByIdAsync(string datingId);
        Task<PagedResult<DatingUserResponse>> GetAllDatingsOfUserAsync(string userId, DatingParameters param);
    }
}