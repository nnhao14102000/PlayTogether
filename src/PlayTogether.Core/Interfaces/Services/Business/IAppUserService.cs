using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IAppUserService
    {
        Task<PersonalInfoResponse> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal);
    }
}