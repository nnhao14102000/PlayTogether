using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Interfaces.Repositories.Business.Charity;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<CharityResponseDto>> GetAllCharityAsync()
        {
            var charities = await _context.Admins.ToListAsync().ConfigureAwait(false);
            if (charities is not null) {
                return _mapper.Map<IEnumerable<CharityResponseDto>>(charities);
            }
            return null;
        }
    }
}
