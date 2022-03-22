using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteNotificationAsync(string id)
        {
            var noti = await _context.Notifications.FindAsync(id);
            if (noti is null || noti.Status is NotificationStatusConstants.NotRead) {
                return false;
            }
            _context.Notifications.Remove(noti);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<NotificationGetAllResponse>> GetAllNotificationsAsync(
            ClaimsPrincipal principal,
            NotificationParameters param)
        {
            var notifications = await _context.Notifications.ToListAsync();
            var query = notifications.AsQueryable();

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if(loggedInUser is not null && user is null){
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

            return null;
        }

        private void FilterByIsReadNotification(ref IQueryable<Entities.Notification> query, bool? isRead)
        {
            if (!query.Any() || isRead is null) {
                return;
            }
            if(isRead is false){
                query = query.Where(x => x.Status == NotificationStatusConstants.NotRead);
            }else{
                query = query.Where(x => x.Status == NotificationStatusConstants.Read);
            }
        }

        private void FilterByReceiverId(ref IQueryable<Entities.Notification> query, string id)
        {
            if (!query.Any() || String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id)) {
                return;
            }
            query = query.Where(x => x.ReceiverId == id);
        }

        private void OrderByCreatedDate(ref IQueryable<Entities.Notification> query, bool? orderByCreatedDate)
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

        public async Task<NotificationGetDetailResponse> GetNotificationByIdAsync(string id)
        {
            var noti = await _context.Notifications.FindAsync(id);
            if (noti is null) {
                return null;
            }

            noti.Status = NotificationStatusConstants.Read;
            if (await _context.SaveChangesAsync() < 0) {
                return null;
            }

            return _mapper.Map<NotificationGetDetailResponse>(noti);
        }
    }
}