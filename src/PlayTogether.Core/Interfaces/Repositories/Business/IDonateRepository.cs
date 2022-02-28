using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IDonateRepository
    {
        Task<int> CalculateDonateAsync(ClaimsPrincipal principal, DonateParameters param);
    }
}
