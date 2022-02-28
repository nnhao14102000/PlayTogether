using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IDonateService
    {
        Task<int> CalculateDonateAsync(ClaimsPrincipal principal, DonateParameters param);
    }
}