using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IAdminService
    {
        Task<Result<(int, int, int, int)>> AdminStatisticAsync();
    }
}
