using PlayTogether.Core.Dtos.Incoming.Business.Ignore;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IIgnoreRepository
    {
        Task<Result<bool>> CreateIgnoreUserAsync(ClaimsPrincipal principal, string toUserId, IgnoreCreateRequest request);
    }
}