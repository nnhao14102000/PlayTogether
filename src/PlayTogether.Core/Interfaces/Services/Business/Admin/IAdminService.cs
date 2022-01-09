using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Admin
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminResponseDto>> GetAllAdminAsync();
    }
}
