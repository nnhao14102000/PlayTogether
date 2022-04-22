using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.CharityWithdraw
{
    public class CharityWithdrawRepository : BaseRepository, ICharityWithdrawRepository
    {
        public CharityWithdrawRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<PagedResult<Core.Entities.CharityWithdraw>> GetAllCharityWithdrawHistoriesAsync(string charityId, CharityWithdrawParameters param)
        {
            var result = new PagedResult<Core.Entities.CharityWithdraw>();
            var charity = await _context.Charities.FindAsync(charityId);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy tài khoản tổ chức từ thiện.");
                return result;
            }
            var withdrawHistories = await _context.CharityWithdraws.Where(x => x.CharityId == charityId).ToListAsync();
            var query = withdrawHistories.AsQueryable();
            FilterHistoryInDayRange(ref query, param.FromDate, param.ToDate);
            OrderByNewHistory(ref query, param.IsNew);
            withdrawHistories = query.ToList();
            return PagedResult<Core.Entities.CharityWithdraw>.ToPagedList(withdrawHistories, param.PageNumber, param.PageSize);
        }

        private void OrderByNewHistory(ref IQueryable<Core.Entities.CharityWithdraw> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterHistoryInDayRange(ref IQueryable<Core.Entities.CharityWithdraw> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }
    }
}