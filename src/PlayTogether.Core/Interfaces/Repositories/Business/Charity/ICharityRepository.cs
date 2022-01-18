using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Charity
{
    public interface ICharityRepository
    {
        Task<IEnumerable<CharityResponse>> GetAllCharityAsync();
    }
}
