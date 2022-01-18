using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Interfaces.Repositories.Business.Player;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Player
{
    public class PlayerRepository: IPlayerRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public PlayerRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<PlayerResponse>> GetAllPlayerAsync()
        {
            var players = await _context.Players.ToListAsync().ConfigureAwait(false);
            if (players is not null) {
                return _mapper.Map<IEnumerable<PlayerResponse>>(players);
            }
            return null;
        }
    }
}
