using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Hirer
{
    public interface IHirerService
    {
        Task<PagedResult<HirerResponse>> GetAllHirersAsync(HirerParameters param);
    }
}
