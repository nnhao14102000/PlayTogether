using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Hirer;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Hirer
{
    public class HirerRepository : IHirerRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public HirerRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<HirerResponse>> GetAllHirersAsync(HirerParameters param)
        {
            List<Entities.Hirer> hirers = null;

            hirers = await _context.Hirers.ToListAsync().ConfigureAwait(false);

            if (!String.IsNullOrEmpty(param.Name)) {
                var query = hirers.AsQueryable();
                query = query.Where(x => (x.Lastname + x.Firstname).ToLower().Contains(param.Name.ToLower()));
                hirers = query.ToList();
            }

            if (hirers is not null) {
                var response = _mapper.Map<List<HirerResponse>>(hirers);
                return PagedResult<HirerResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }
            
            return null;
        }
    }
}
