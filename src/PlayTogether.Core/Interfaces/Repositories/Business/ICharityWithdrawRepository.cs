using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Entities;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface ICharityWithdrawRepository
    {
        Task<PagedResult<CharityWithdraw>> GetAllCharityWithdrawHistoriesAsync(string charityId, CharityWithdrawParameters param);
    }
}