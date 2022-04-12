using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PlayTogether.Core.Parameters;

namespace PlayTogether.Infrastructure.Repositories.Business.Dating
{
    public class DatingRepository : BaseRepository, IDatingRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DatingRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null || user.IsActive is false || user.Status is not UserStatusConstants.Online) {
                return false;
            }
            if (request.FromHour >= request.ToHour) {
                return false;
            }

            if (request.DayInWeek == 2) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 2).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }

            }
            else if (request.DayInWeek == 3) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 3).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }
            }
            else if (request.DayInWeek == 4) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 4).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }
            }
            else if (request.DayInWeek == 5) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 5).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }
            }
            else if (request.DayInWeek == 6) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 6).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }
            }
            else if (request.DayInWeek == 7) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 7).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }
            }
            else if (request.DayInWeek == 8) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 8).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        return false;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        return false;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        return false;
                    }
                }
            }
            else {
                return false;
            }

            var model = _mapper.Map<Entities.Dating>(request);
            model.UserId = user.Id;
            await _context.Datings.AddAsync(model);

            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeleteDatingAsync(ClaimsPrincipal principal, string datingId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null || user.IsActive is false || user.Status is not UserStatusConstants.Online) {
                return false;
            }

            var dating = await _context.Datings.FindAsync(datingId);
            if (dating is null || dating.UserId != user.Id) {
                return false;
            }
            _context.Datings.Remove(dating);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<DatingUserResponse>> GetAllDatingsOfUserAsync(string userId, DatingParameters param)
        {
            var result = new PagedResult<DatingUserResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if(user is null){
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }
            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.DisableUser);
                return result;
            }
            var datings = await _context.Datings.Where(x => x.UserId == user.Id).ToListAsync();
            var response = _mapper.Map<List<DatingUserResponse>>(datings);
            return PagedResult<DatingUserResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<Result<DatingUserResponse>> GetDatingByIdAsync(string datingId)
        {
            var result = new Result<DatingUserResponse>();
            var dating = await _context.Datings.FindAsync(datingId);
            if(dating is null){
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + " khung thời gian này.");
                return result;
            }
            var response = _mapper.Map<DatingUserResponse>(dating);
            result.Content = response;
            return result;
        }
    }
}