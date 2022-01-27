using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IAdminService
    {
        Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param);
        
    }
}
