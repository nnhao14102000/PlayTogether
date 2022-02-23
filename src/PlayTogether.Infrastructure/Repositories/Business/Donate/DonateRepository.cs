using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace PlayTogether.Infrastructure.Repositories.Business.Donate
{
    public class DonateRepository : BaseRepository, IDonateRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DonateRepository(IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<int> CalculateTurnDonateInDayAsync(ClaimsPrincipal principal)
        {
            var toDay = DateTime.Now;
            var dateTime = toDay.ToString().Split(" ");
            var date = dateTime[0];

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return -1;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (charity is null) {
                return -1;
            }

            var donate = await (from d in _context.Donates
                                where d.CharityId == charity.Id && d.CreatedDate.ToString().Contains(date)
                                select d).ToListAsync();

            return donate.Count;
        }
    }
}