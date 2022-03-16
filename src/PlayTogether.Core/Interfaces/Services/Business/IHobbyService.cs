using PlayTogether.Core.Dtos.Incoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IHobbyService
    {
        Task<bool> CreateHobbiesAsync(ClaimsPrincipal principal, List<HobbyCreateRequest> requests);
        Task<PagedResult<HobbiesGetAllResponse>> GetAllHobbiesAsync(string userId, HobbyParameters param);
        Task<bool> DeleteHobbyAsync(ClaimsPrincipal principal, string hobbyId);
    }
}