using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Security.Claims;
using System.Threading.Tasks;

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

        
    }
}