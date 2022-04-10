using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

            if (request.IsMON is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsMON == true).ToListAsync();
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
            else if (request.IsTUE is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsTUE == true).ToListAsync();
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
            else if (request.IsWED is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsWED == true).ToListAsync();
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
            else if (request.IsTHU is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsTHU == true).ToListAsync();
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
            else if (request.IsFRI is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsFRI == true).ToListAsync();
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
            else if (request.IsSAT is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsSAT == true).ToListAsync();
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
            else if (request.IsSUN is true) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.IsSUN == true).ToListAsync();
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
    }
}