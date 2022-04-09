using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Hobby;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Hobby
{
    public class HobbyRepository : BaseRepository, IHobbyRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HobbyRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateHobbiesAsync(ClaimsPrincipal principal, List<HobbyCreateRequest> requests)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null || user.IsActive is false || user.Status is not UserStatusConstants.Online) {
                return false;
            }

            foreach (var request in requests) {
                var game = await _context.Games.FindAsync(request.GameId);
                if (game is null) {
                    return false;
                }

                var existGameInHobby = await _context.Hobbies.Where(x => x.GameId == request.GameId && x.UserId == user.Id).ToListAsync();
                if (existGameInHobby.Count == 1) continue;

                var model = _mapper.Map<Entities.Hobby>(request);
                model.UserId = user.Id;
                await _context.Hobbies.AddAsync(model);
                if (await _context.SaveChangesAsync() < 0) {
                    return false;
                }
            }

            return true;
        }

        public async Task<PagedResult<HobbiesGetAllResponse>> GetAllHobbiesAsync(string userId, HobbyParameters param)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) return null;
            var hobbies = await _context.Hobbies.Where(x => x.UserId == user.Id).ToListAsync();
            foreach (var hobby in hobbies) {
                await _context.Entry(hobby).Reference(x => x.User).LoadAsync();
                await _context.Entry(hobby).Reference(x => x.Game).LoadAsync();
            }
            var response = _mapper.Map<List<HobbiesGetAllResponse>>(hobbies);
            return PagedResult<HobbiesGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<bool> DeleteHobbyAsync(ClaimsPrincipal principal, string hobbyId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null || user.IsActive is false || user.Status is not UserStatusConstants.Online) {
                return false;
            }

            var hobby = await _context.Hobbies.FindAsync(hobbyId);
            if (hobby is null) return false;
            if (hobby.UserId != user.Id) return false;
            _context.Hobbies.Remove(hobby);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeleteRangesHobbiesAsync(ClaimsPrincipal principal, List<HobbyDeleteRequest> hobbyIds)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null || user.IsActive is false || user.Status is not UserStatusConstants.Online) {
                return false;
            }
            var list = new List<Entities.Hobby>();
            foreach (var hobbyId in hobbyIds) {
                var hobby = await _context.Hobbies.FindAsync(hobbyId.HobbyId);
                if (hobby is null) return false;
                if (hobby.UserId != user.Id) return false;
                list.Add(hobby);
            }

            _context.Hobbies.RemoveRange(list);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}