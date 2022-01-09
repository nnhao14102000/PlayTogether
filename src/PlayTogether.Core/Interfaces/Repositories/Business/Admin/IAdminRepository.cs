using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Admin
{
    public interface IAdminRepository
    {
        Task<IEnumerable<AdminResponseDto>> GetAllAdminAsync();
    }
}
