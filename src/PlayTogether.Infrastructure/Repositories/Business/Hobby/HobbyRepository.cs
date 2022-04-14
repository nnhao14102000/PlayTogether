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
using System;
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

        public async Task<Result<bool>> CreateHobbiesAsync(ClaimsPrincipal principal, List<HobbyCreateRequest> requests)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            foreach (var request in requests) {
                var game = await _context.Games.FindAsync(request.GameId);
                if (game is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" game.");
                    return result;
                }

                var existGameInHobby = await _context.Hobbies.Where(x => x.GameId == request.GameId && x.UserId == user.Id).ToListAsync();
                if (existGameInHobby.Count == 1) continue;

                var model = _mapper.Map<Core.Entities.Hobby>(request);
                model.UserId = user.Id;
                await _context.Hobbies.AddAsync(model);
                if (await _context.SaveChangesAsync() < 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }
            result.Content = true;
            return result;
        }

        public async Task<PagedResult<HobbiesGetAllResponse>> GetAllHobbiesAsync(string userId, HobbyParameters param)
        {
            var result = new PagedResult<HobbiesGetAllResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }
            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.DisableUser);
                return result;
            }
            var hobbies = await _context.Hobbies.Where(x => x.UserId == user.Id).ToListAsync();
            var query = hobbies.AsQueryable();
            OrderByCreatedDate(ref query, param.IsNew);
            hobbies = query.ToList();
            foreach (var hobby in hobbies) {
                await _context.Entry(hobby).Reference(x => x.User).LoadAsync();
                await _context.Entry(hobby).Reference(x => x.Game).LoadAsync();
            }
            var response = _mapper.Map<List<HobbiesGetAllResponse>>(hobbies);
            return PagedResult<HobbiesGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void OrderByCreatedDate(ref IQueryable<Core.Entities.Hobby> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew == true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        public async Task<Result<bool>> DeleteHobbyAsync(ClaimsPrincipal principal, string hobbyId)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            var hobby = await _context.Hobbies.FindAsync(hobbyId);
            if (hobby is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" hobby.");
                return result;
            }
            if (hobby.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }
            _context.Hobbies.Remove(hobby);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteRangesHobbiesAsync(ClaimsPrincipal principal, List<HobbyDeleteRequest> hobbyIds)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hiện tài khoản bạn đang ở chế độ {user.Status}. Vui lòng thử lại sau khi tất cả các thao tác hoàn tất.");
                return result;
            }

            var list = new List<Core.Entities.Hobby>();
            foreach (var hobbyId in hobbyIds) {
                var hobby = await _context.Hobbies.FindAsync(hobbyId.HobbyId);
                if (hobby is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" hobby.");
                    return result;
                }
                if (hobby.UserId != user.Id) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                    return result;
                }
                list.Add(hobby);
            }

            _context.Hobbies.RemoveRange(list);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}