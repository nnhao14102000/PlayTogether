using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Charity
{
    public interface ICharityService
    {
        Task<IEnumerable<CharityResponseDto>> GetAllCharityAsync();
    }
}
