using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Hirer
{
    public interface IHirerService
    {
        Task<IEnumerable<HirerResponse>> GetAllHirerAsync();
    }
}
