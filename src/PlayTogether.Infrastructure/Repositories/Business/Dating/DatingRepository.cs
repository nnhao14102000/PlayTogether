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
using System;

namespace PlayTogether.Infrastructure.Repositories.Business.Dating
{
    public class DatingRepository : BaseRepository, IDatingRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public DatingRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> CreateDatingAsync(ClaimsPrincipal principal, DatingCreateRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

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
            if (request.FromHour >= request.ToHour) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Giờ bắt đầu {request.FromHour} nên thấp hơn giờ kết thúc {request.ToHour}");
                return result;
            }

            if (request.DayInWeek == 2) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 2).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Hai của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Hai của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Hai của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày thứ Hai của bạn");
                        return result;
                    }
                }

            }
            else if (request.DayInWeek == 3) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 3).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Ba của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Ba của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Ba của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày thứ Ba của bạn");
                        return result;
                    }
                }
            }
            else if (request.DayInWeek == 4) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 4).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Tư của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Tư của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Tư của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày thứ Tư của bạn");
                        return result;
                    }
                }
            }
            else if (request.DayInWeek == 5) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 5).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Năm của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Năm của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Năm của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày thứ Năm của bạn");
                        return result;
                    }
                }
            }
            else if (request.DayInWeek == 6) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 6).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Sáu của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Sáu của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Sáu của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày thứ Sáu của bạn");
                        return result;
                    }
                }
            }
            else if (request.DayInWeek == 7) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 7).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Bảy của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Bảy của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ Bảy của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày thứ Bảy của bạn");
                        return result;
                    }
                }
            }
            else if (request.DayInWeek == 8) {
                var datings = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == 8).ToListAsync();
                foreach (var item in datings) {
                    if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật của bạn");
                        return result;
                    }
                    if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.FromHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật của bạn");
                        return result;
                    }
                    if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Mốc giờ {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật của bạn");
                        return result;
                    }
                    if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Khung giờ {request.FromHour} tới {request.ToHour} đang bao phủ 1 hoặc nhiều khung giờ khác. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật của bạn");
                        return result;
                    }
                }
            }

            var model = _mapper.Map<Entities.Dating>(request);
            model.UserId = user.Id;
            await _context.Datings.AddAsync(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteDatingAsync(ClaimsPrincipal principal, string datingId)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

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

            var dating = await _context.Datings.FindAsync(datingId);
            if (dating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" khung giờ này.");
                return result;
            }

            if (dating.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }
            _context.Datings.Remove(dating);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<DatingUserResponse>> GetAllDatingsOfUserAsync(string userId, DatingParameters param)
        {
            var result = new PagedResult<DatingUserResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }
            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.DisableUser);
                return result;
            }
            var datings = await _context.Datings.Where(x => x.UserId == user.Id).ToListAsync();
            var query = datings.AsQueryable();
            SortByDay(ref query, param.SortByDay);
            datings = query.ToList();
            var response = _mapper.Map<List<DatingUserResponse>>(datings);
            return PagedResult<DatingUserResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void SortByDay(ref IQueryable<Entities.Dating> query, bool? sortByDay)
        {
            if (!query.Any() || sortByDay is null) {
                return;
            }
            if (sortByDay is true) {
                query = query.OrderBy(x => x.DayInWeek);
            }
            else {
                query = query.OrderByDescending(x => x.DayInWeek);
            }
        }

        public async Task<Result<DatingUserResponse>> GetDatingByIdAsync(string datingId)
        {
            var result = new Result<DatingUserResponse>();
            var dating = await _context.Datings.FindAsync(datingId);
            if (dating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + " khung thời gian này.");
                return result;
            }
            var response = _mapper.Map<DatingUserResponse>(dating);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateDatingAsync(ClaimsPrincipal principal, string datingId, DatingUpdateRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

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
            if (request.FromHour >= request.ToHour) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Giờ bắt đầu {request.FromHour} nên thấp hơn giờ kết thúc {request.ToHour}");
                return result;
            }

            var dating = await _context.Datings.FindAsync(datingId);
            if (dating is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy khung thời gian này.");
                return result;
            }

            if (dating.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            var datingsInDay = await _context.Datings.Where(x => x.UserId == user.Id && x.DayInWeek == dating.DayInWeek && x.Id != dating.Id).ToListAsync();
            foreach (var item in datingsInDay) {
                if (request.FromHour >= item.FromHour && request.ToHour <= item.ToHour) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, item.DayInWeek < 8 ? $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ {item.DayInWeek}" : $"Khung giờ {request.FromHour/60}h{request.FromHour%60}p tới {request.ToHour/60}h{request.ToHour%60}p đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật");
                    return result;
                }
                if (request.FromHour >= item.FromHour && request.FromHour <= item.ToHour) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, item.DayInWeek < 8 ? $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ {item.DayInWeek}" : $"Khung giờ {request.FromHour/60}h{request.FromHour%60}p tới {request.ToHour/60}h{request.ToHour%60}p đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật");
                    return result;
                }
                if (request.ToHour >= item.FromHour && request.ToHour <= item.ToHour) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, item.DayInWeek < 8 ? $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ {item.DayInWeek}" : $"Khung giờ {request.FromHour/60}h{request.FromHour%60}p tới {request.ToHour/60}h{request.ToHour%60}p đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật");
                    return result;
                }
                if (request.FromHour < item.FromHour && request.ToHour > item.ToHour) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, item.DayInWeek < 8 ? $"Khung giờ {request.FromHour} tới {request.ToHour} đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày thứ {item.DayInWeek}" : $"Khung giờ {request.FromHour/60}h{request.FromHour%60}p tới {request.ToHour/60}h{request.ToHour%60}p đã tồn tại hoặc bị khung giờ khác bao phủ. Vui lòng kiểm tra lại các khung giờ ngày Chủ Nhật");
                    return result;
                }
            }

            var model = _mapper.Map<Entities.Dating>(request);
            model.UserId = user.Id;
            await _context.Datings.AddAsync(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}