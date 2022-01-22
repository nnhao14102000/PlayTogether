using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Hirer
{
    public interface IHirerRepository
    {
        Task<PagedResult<HirerResponse>> GetAllHirersAsync(HirerParameters param);
    }
}
