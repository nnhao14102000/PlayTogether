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
using System.Globalization;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;

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

        public async Task<int> CalculateDonateAsync(ClaimsPrincipal principal, DonateParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return -1;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (charity is null) {
                return -1;
            }

            var donate = await _context.Donates.Where(x => x.CharityId == charity.Id).ToListAsync();
            int count = -1;
            count = CountNumberOfDonateInDay(donate, param.IsCalculateNumberOfDonateInDate);
            return count;
        }

        private int CountNumberOfDonateInDay(List<Entities.Donate> donates, bool? isCalculateNumberOfDonateInDate)
        {
            if(!donates.Any() || isCalculateNumberOfDonateInDate is null || isCalculateNumberOfDonateInDate is false) {
                return 0;
            }
            string strDate = DateTime.Now.ToString("M/dd/yyyy");
            int count = 0;
            foreach (var item in donates)
            {
                var date = item.CreatedDate.ToString();
                if(date.Contains(strDate)){
                    count ++;
                }
            }
            return count;
        }
    }
}