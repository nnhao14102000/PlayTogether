using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Player;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public PlayerRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<PlayerResponse>> GetAllPlayersAsync(PlayerParameters param)
        {
            List<Entities.Player> players = null;

            if (param.Gender is not null) {
                players = await _context.Players.Where(x => x.Gender == param.Gender).ToListAsync();
            }
            else {

                players = await _context.Players.ToListAsync().ConfigureAwait(false);
            }

            if (!String.IsNullOrEmpty(param.Name)) {
                var query = players.AsQueryable();
                query = query.Where(players => (players.Lastname + players.Firstname).ToLower().Contains(param.Name.ToLower()));
                players = query.ToList();
            }

            if (players is not null) {
                var response = _mapper.Map<List<PlayerResponse>>(players);
                return PagedResult<PlayerResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }
            
            return null;
        }
    }
}
