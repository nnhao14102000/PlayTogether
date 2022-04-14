using PlayTogether.Core.Dtos.Incoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IDonateService
    {
        Task<Result<(int, float, int, float)>> CalculateDonateAsync(ClaimsPrincipal principal);
        Task<Result<bool>> CreateDonateAsync(ClaimsPrincipal principal, string charityId, DonateCreateRequest request);
        Task<PagedResult<DonateResponse>> GetAllDonatesAsync(ClaimsPrincipal principal, DonateParameters param);
        Task<Result<DonateResponse>> GetDonateByIdAsync(string donateId);
    }
}