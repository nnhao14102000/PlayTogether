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

        // public async Task<(int, float, int, float)> CalculateDonateAsync(ClaimsPrincipal principal)
        // {
        //     var loggedInUser = await _userManager.GetUserAsync(principal);
        //     if (loggedInUser is null) {
        //         return (-1, -1, -1, -1);
        //     }
        //     var identityId = loggedInUser.Id;

        //     var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

        //     if (charity is null) {
        //         return (-1, -1, -1, -1);
        //     }

        //     var donate = await _context.Donates.Where(x => x.CharityId == charity.Id).ToListAsync();
        //     int countNumberOfDonateInDay = CountNumberOfDonateInDay(donate);
        //     float totalMoneyDonatedInDay = TotalMoneyDonateReceiveInDay(donate);
        //     int countTotalNumberOfPeopleDonateInDay = 0;
        //     int count4 = 0;
            
        //     return (countNumberOfDonateInDay, totalMoneyDonatedInDay, countTotalNumberOfPeopleDonateInDay, count4);
        // }

        // private int CountNumberOfDonateInDay(List<Entities.Donate> donates)
        // {
        //     if(!donates.Any()) {
        //         return 0;
        //     }
        //     string strDate = DateTime.Now.ToString("dd/MM/yyyy");
        //     int count = 0;
        //     foreach (var item in donates)
        //     {
        //         var date = item.CreatedDate?.ToString("dd/MM/yyyy");
        //         if(date.Contains(strDate)){
        //             count ++;
        //         }
        //     }
        //     return count;
        // }

        // private int CountTotalNumberOfPeopleDonateInDay(List<Entities.Donate> donates){
        //     if(!donates.Any()) {
        //         return 0;
        //     }
        //     int count = 0;

        //     return count;
        // }

        // private float TotalMoneyDonateReceiveInDay(List<Entities.Donate> donates)
        // {
        //     if(!donates.Any()) {
        //         return 0;
        //     }
        //     float total = 0;
        //     string strDate = DateTime.Now.ToString("dd/MM/yyyy");
        //     foreach (var item in donates)
        //     {
        //         _context.Entry(item).Reference(x => x.Order).Load();
        //         var date = item.CreatedDate?.ToString("dd/MM/yyyy");
        //         if(date.Contains(strDate)){
        //             total += item.Order.TotalPrices;
        //         }
        //     }

        //     return total;
        // }
    }
}