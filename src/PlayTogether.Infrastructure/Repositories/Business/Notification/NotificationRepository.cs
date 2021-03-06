using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Notification;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Notification
{
    public class NotificationRepository : BaseRepository, INotificationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public NotificationRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> DeleteNotificationAsync(string id)
        {
            var result = new Result<bool>();
            var noti = await _context.Notifications.FindAsync(id);
            if (noti is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thông báo.");
                return result;
            }

            if (noti.Status is NotificationStatusConstants.NotRead) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReadNoti);
                return result;
            }
            _context.Notifications.Remove(noti);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<NotificationGetAllResponse>> GetAllNotificationsAsync(
            ClaimsPrincipal principal,
            NotificationParameters param)
        {
            var result = new PagedResult<NotificationGetAllResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var notifications = await _context.Notifications.ToListAsync();
            var query = notifications.AsQueryable();

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (loggedInUser is not null && user is null && charity is null) {
                FilterByReceiverId(ref query, "");
                FilterByIsReadNotification(ref query, param.IsRead);
                OrderByCreatedDate(ref query, param.IsNew);

                notifications = query.ToList();
                var response = _mapper.Map<List<NotificationGetAllResponse>>(notifications);
                return PagedResult<NotificationGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            if (user is not null) {
                FilterByReceiverId(ref query, user.Id);
                FilterByIsReadNotification(ref query, param.IsRead);
                OrderByCreatedDate(ref query, param.IsNew);

                notifications = query.ToList();
                var response = _mapper.Map<List<NotificationGetAllResponse>>(notifications);
                return PagedResult<NotificationGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            if (charity is not null) {
                FilterByReceiverId(ref query, charity.Id);
                FilterByIsReadNotification(ref query, param.IsRead);
                OrderByCreatedDate(ref query, param.IsNew);

                notifications = query.ToList();
                var response = _mapper.Map<List<NotificationGetAllResponse>>(notifications);
                return PagedResult<NotificationGetAllResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Có lỗi xảy ra. Bạn vui lòng đăng nhập và thử lại.");
            return result;
        }

        private void FilterByIsReadNotification(ref IQueryable<Core.Entities.Notification> query, bool? isRead)
        {
            if (!query.Any() || isRead is null) {
                return;
            }
            if (isRead is false) {
                query = query.Where(x => x.Status == NotificationStatusConstants.NotRead);
            }
            else {
                query = query.Where(x => x.Status == NotificationStatusConstants.Read);
            }
        }

        private void FilterByReceiverId(ref IQueryable<Core.Entities.Notification> query, string id)
        {
            if (!query.Any() || String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id)) {
                return;
            }
            query = query.Where(x => x.ReceiverId == id);
        }

        private void OrderByCreatedDate(ref IQueryable<Core.Entities.Notification> query, bool? orderByCreatedDate)
        {
            if (!query.Any() || orderByCreatedDate is null) {
                return;
            }

            if (orderByCreatedDate is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        public async Task<Result<NotificationGetDetailResponse>> GetNotificationByIdAsync(string id)
        {
            var result = new Result<NotificationGetDetailResponse>();
            var noti = await _context.Notifications.FindAsync(id);
            if (noti is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thông báo.");
                return result;
            }

            noti.Status = NotificationStatusConstants.Read;
            if (await _context.SaveChangesAsync() < 0) {
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }

            var response = _mapper.Map<NotificationGetDetailResponse>(noti);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> CreateNotificationAsync(NotificationCreateRequest request)
        {
            var result = new Result<bool>();
            var user = await _context.AppUsers.FindAsync(request.ReceiverId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" thông báo.");
                return result;
            }
            var model = _mapper.Map<Core.Entities.Notification>(request);
            await _context.Notifications.AddAsync(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> CreateNotificationAllServerAsync(NotificationCreateAllServerRequest request)
        {
            var result = new Result<bool>();
            var users = await _context.AppUsers.ToListAsync();
            var listModel = new List<Core.Entities.Notification>();
            
            foreach (var user in users) {
                var model = _mapper.Map<Core.Entities.Notification>(request);
                model.ReceiverId = user.Id;
                listModel.Add(model);
            }
            await _context.Notifications.AddRangeAsync(listModel);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}