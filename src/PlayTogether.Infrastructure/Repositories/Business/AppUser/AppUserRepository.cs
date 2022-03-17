using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.AppUser
{
    public class AppUserRepository : BaseRepository, IAppUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AppUserRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<bool> ChangeIsPlayerAsync(ClaimsPrincipal principal, UserIsPlayerChangeRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            if (request.IsPlayer is true) {
                if (user.IsActive is false) return false;

                if (user.Status is UserStatusConstants.Offline) return false;

                if (user.PricePerHour < ValueConstants.PricePerHourMinValue) return false;

                if (user.MaxHourHire < ValueConstants.MaxHourHireMinValue
                    || user.MaxHourHire > ValueConstants.MaxHourHireMaxValue) return false;

                var gameOfUserExist = await _context.GameOfUsers.AnyAsync(x => x.UserId == user.Id);
                if (!gameOfUserExist) return false;

            }
            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PersonalInfoResponse> GetPersonalInfoByIdentityIdAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return null;
            }

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();
            await _context.Entry(user).Collection(x => x.Images).LoadAsync();

            return _mapper.Map<PersonalInfoResponse>(user);
        }

        public async Task<bool> UpdateUserServiceInfoAsync(ClaimsPrincipal principal, UserInfoForIsPlayerUpdateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdatePersonalInfoAsync(ClaimsPrincipal principal, UserPersonalInfoUpdateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            _mapper.Map(request, user);
            _context.AppUsers.Update(user);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<UserGetBasicInfoResponse> GetUserBasicInfoByIdAsync(string userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                return null;
            }
            return _mapper.Map<UserGetBasicInfoResponse>(user);
        }

        public async Task<UserGetServiceInfoResponse> GetUserServiceInfoByIdAsync(string userId)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                return null;
            }
            return _mapper.Map<UserGetServiceInfoResponse>(user);
        }
    }
}