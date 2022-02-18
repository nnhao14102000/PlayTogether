using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Hirer
{
    public class HirerRepository : BaseRepository, IHirerRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HirerRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<HirerGetAllResponseForAdmin>> GetAllHirersForAdminAsync(HirerParameters param)
        {
            List<Entities.Hirer> hirers = null;

            hirers = await _context.Hirers.ToListAsync().ConfigureAwait(false);

            if (hirers is not null) {
                if (!String.IsNullOrEmpty(param.Name)) {
                    var query = hirers.AsQueryable();
                    query = query.Where(x => (x.Lastname + x.Firstname).ToLower()
                                                                       .Contains(param.Name.ToLower()));
                    hirers = query.ToList();
                }

                var response = _mapper.Map<List<HirerGetAllResponseForAdmin>>(hirers);
                return PagedResult<HirerGetAllResponseForAdmin>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
        }

        public async Task<HirerGetByIdResponse> GetHirerByIdForHirerAsync(string id)
        {
            var hirer = await _context.Hirers.FindAsync(id);

            if (hirer is null) {
                return null;
            }
            return _mapper.Map<HirerGetByIdResponse>(hirer);
        }

        public async Task<HirerGetProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var hirerProfile = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            return _mapper.Map<HirerGetProfileResponse>(hirerProfile);
        }

        public async Task<bool> UpdateHirerInformationAsync(string id, HirerInfoUpdateRequest request)
        {
            var hirer = await _context.Hirers.FindAsync(id);
            if (hirer is null) {
                return false;
            }
            _mapper.Map(request, hirer);
            _context.Hirers.Update(hirer);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
