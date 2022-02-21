using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface INotificationRepository
    {
        Task<PagedResult<NotificationGetResponse>> GetAllNotificationsAsync(ClaimsPrincipal principal, NotificationParameters param);

        Task<NotificationGetResponse> GetNotificationByIdAsync(string id);

        Task<bool> DeleteNotificationAsync(string id);
    }
}