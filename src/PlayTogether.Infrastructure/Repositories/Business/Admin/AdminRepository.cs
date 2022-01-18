using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Interfaces.Repositories.Business.Admin;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<AdminResponse>> GetAllAdminAsync()
        {
            var admins = await _context.Admins.ToListAsync().ConfigureAwait(false);
            if (admins is not null) {
                return _mapper.Map<IEnumerable<AdminResponse>>(admins);
            }
            return null;
        }
    }
}
