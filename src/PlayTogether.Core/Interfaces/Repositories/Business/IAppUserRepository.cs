using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IAppUserRepository
    {
        Task<PersonalInfoResponse> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal);
    }
}