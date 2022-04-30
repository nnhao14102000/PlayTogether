using PlayTogether.Core.Dtos.Incoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface INotificationRepository
    {
        Task<PagedResult<NotificationGetAllResponse>> GetAllNotificationsAsync(ClaimsPrincipal principal, NotificationParameters param);

        Task<Result<NotificationGetDetailResponse>> GetNotificationByIdAsync(string id);

        Task<Result<bool>> DeleteNotificationAsync(string id);

        Task<Result<bool>> CreateNotificationAsync(NotificationCreateRequest request);
        Task<Result<bool>> CreateNotificationAllServerAsync(NotificationCreateAllServerRequest request);
    }
}