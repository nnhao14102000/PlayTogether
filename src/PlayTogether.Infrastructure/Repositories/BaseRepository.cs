using AutoMapper;
using PlayTogether.Infrastructure.Data;
using System;

namespace PlayTogether.Infrastructure.Repositories
{
    public class BaseRepository
    {
        protected readonly IMapper _mapper;
        protected readonly AppDbContext _context;

        public BaseRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}