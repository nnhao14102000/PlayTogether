using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Entities;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface ICharityWithdrawService
    {
        Task<PagedResult<CharityWithdraw>> GetAllCharityWithdrawHistoriesAsync(string charityId, CharityWithdrawParameters param);
    }
}