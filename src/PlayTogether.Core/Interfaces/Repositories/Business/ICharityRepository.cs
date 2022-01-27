using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ICharityRepository
    {
        Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param);
    }
}
