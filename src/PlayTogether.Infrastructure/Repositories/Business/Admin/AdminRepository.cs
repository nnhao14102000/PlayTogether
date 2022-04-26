using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Admin
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<(int, int, int, int)>> AdminStatisticAsync()
        {
            var result = new Result<(int, int, int, int)>();
            int numOfReport = 0;
            int numOfDisableUser = 0;
            int numOfSuggestFeedback = 0;
            int numOfNewUser = 0;

            numOfReport = await _context.Reports.Where(x => x.IsApprove == null).CountAsync();
            numOfDisableUser = await _context.AppUsers.Where(x => x.IsActive == false).CountAsync();
            numOfSuggestFeedback = await _context.SystemFeedbacks
                                                    .Where(x => x.IsApprove == null).CountAsync();
            var users = await _context.AppUsers.ToListAsync();
            foreach (var item in users)
            {
                var date = item.CreatedDate.ToShortDateString();
                var toDay = DateTime.UtcNow.AddHours(7).ToShortDateString();
                if(date.Equals(toDay)){
                    numOfNewUser += 1;
                }
            }
            result.Content = (numOfReport, numOfDisableUser, numOfSuggestFeedback, numOfNewUser);
            return result;
        }

        // public async Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param)
        // {
        //     List<Entities.Admin> admins = null;

        //     admins = await _context.Admins.ToListAsync();

        //     if (admins is not null) {
        //         if (!String.IsNullOrEmpty(param.Name)) {
        //             var query = admins.AsQueryable();
        //             query = query.Where(x => (x.Lastname + x.Firstname).ToLower()
        //                                                                .Contains(param.Name.ToLower()));
        //             admins = query.ToList();
        //         }

        //         var response = _mapper.Map<List<AdminResponse>>(admins);
        //         return PagedResult<AdminResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        //     }

        //     return null;
        // }


    }
}
