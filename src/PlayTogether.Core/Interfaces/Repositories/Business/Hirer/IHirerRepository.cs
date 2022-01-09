using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Hirer
{
    public interface IHirerRepository
    {
        Task<IEnumerable<HirerResponseDto>> GetAllHirerAsync();
    }
}
