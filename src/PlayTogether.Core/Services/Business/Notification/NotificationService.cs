using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> CreateNotificationAsync(NotificationCreateRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _notificationRepository.CreateNotificationAsync(request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateNotificationAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> DeleteNotificationAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _notificationRepository.DeleteNotificationAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteNotificationAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<NotificationGetAllResponse>> GetAllNotificationsAsync(
            ClaimsPrincipal principal,
            NotificationParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _notificationRepository.GetAllNotificationsAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllNotificationsAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<NotificationGetDetailResponse>> GetNotificationByIdAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _notificationRepository.GetNotificationByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetNotificationByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}