using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Admin
{
    public interface IAdminRepository
    {
        Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param);
    }
}
