using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Charity
{
    public class CharityRepository : ICharityRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public CharityRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param)
        {
            List<Entities.Charity> charities = null;

            charities = await _context.Charities.ToListAsync();

            if (charities is not null) {
                if (!String.IsNullOrEmpty(param.Name)) {
                    var query = charities.AsQueryable();
                    query = query.Where(x => x.OrganizationName.ToLower().Contains(param.Name.ToLower()));
                    charities = query.ToList();
                }

                var response = _mapper.Map<List<CharityResponse>>(charities);
                return PagedResult<CharityResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
        }
    }
}
