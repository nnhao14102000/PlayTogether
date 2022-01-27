using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class AdminRepository : IAdminRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public AdminRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param)
        {
            List<Entities.Admin> admins = null;

            admins = await _context.Admins.ToListAsync();

            if (admins is not null) {
                if (!String.IsNullOrEmpty(param.Name)) {
                    var query = admins.AsQueryable();
                    query = query.Where(x => (x.Lastname + x.Firstname).ToLower().Contains(param.Name.ToLower()));
                    admins = query.ToList();
                }

                var response = _mapper.Map<List<AdminResponse>>(admins);
                return PagedResult<AdminResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
        }


    }
}
