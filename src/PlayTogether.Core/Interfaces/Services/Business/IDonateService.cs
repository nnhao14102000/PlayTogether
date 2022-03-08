using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IDonateService
    {
        Task<(int, float, int, float)> CalculateDonateAsync(ClaimsPrincipal principal);
    }
}