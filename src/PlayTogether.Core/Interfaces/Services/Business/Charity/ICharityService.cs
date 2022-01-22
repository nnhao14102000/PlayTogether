using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Charity
{
    public interface ICharityService
    {
        Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param);
    }
}
