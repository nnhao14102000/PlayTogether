using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Charity
{
    public class CharityRepository : BaseRepository, ICharityRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public CharityRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param)
        {
            var charities = await _context.Charities.ToListAsync();
            var queryCharity = charities.AsQueryable();

            FilterActiveCharities(ref queryCharity, param.IsActive);
            FilterCharitiesByName(ref queryCharity, param.Name);

            charities = queryCharity.ToList();
            var response = _mapper.Map<List<CharityResponse>>(charities);
            return PagedResult<CharityResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterCharitiesByName(ref IQueryable<Core.Entities.Charity> queryCharity, string name)
        {
            if (!queryCharity.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            queryCharity = queryCharity.Where(x => x.OrganizationName.ToLower().Contains(name.ToLower()));
        }

        private void FilterActiveCharities(ref IQueryable<Core.Entities.Charity> queryCharity, bool? isActive)
        {
            if (!queryCharity.Any() || isActive is null) {
                return;
            }
            queryCharity = queryCharity.Where(x => x.IsActive == isActive);
        }

        public async Task<CharityResponse> GetCharityByIdAsync(string id)
        {
            var charity = await _context.Charities.FindAsync(id);
            if (charity is null) {
                return null;
            }
            return _mapper.Map<CharityResponse>(charity);
        }

        public async Task<bool> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request)
        {
            var charity = await _context.Charities.FindAsync(charityId);
            if (charity is null) {
                return false;
            }
            var model = _mapper.Map(request, charity);
            _context.Charities.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<CharityResponse> GetProfileAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (charity is null) {
                return null;
            }
            return _mapper.Map<CharityResponse>(charity);
        }

        public async Task<bool> UpdateProfileAsync(ClaimsPrincipal principal, string charityId, CharityUpdateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (charity is null || charity.Id != charityId) {
                return false;
            }
            if (charity is null) return false;
            var model = _mapper.Map(request, charity);
            _context.Charities.Update(model);
            return (await _context.SaveChangesAsync() >= 0);

        }

        public async Task<bool> CharityWithDrawAsync(ClaimsPrincipal principal, CharityWithDrawRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (charity is null) {
                return false;
            }

            var model = _mapper.Map<Core.Entities.CharityWithdraw>(request);
            model.CharityId = charity.Id;
            await _context.CharityWithdraws.AddAsync(model);

            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
