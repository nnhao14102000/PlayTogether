using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IAdminRepository
    {
        Task<Result<(int, int, int, int)>> AdminStatisticAsync();
        Task<Result<bool>> MaintainAsync();
    }
}
