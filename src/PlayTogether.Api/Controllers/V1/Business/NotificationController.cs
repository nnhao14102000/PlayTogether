using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get all notifications 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, Charity, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + ","
                        + AuthConstant.RoleCharity + ","
                        + AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<NotificationGetAllResponse>>> GetAllNotifications(
            [FromQuery] NotificationParameters param)
        {
            var response = await _notificationService.GetAllNotificationsAsync(HttpContext.User, param);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        /// <summary>
        /// Get notification by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, Charity, User
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + ","
                        + AuthConstant.RoleCharity + ","
                        + AuthConstant.RoleUser)]
        public async Task<ActionResult<NotificationGetDetailResponse>> GetNotificationById(string id)
        {
            var response = await _notificationService.GetNotificationByIdAsync(id);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return Ok(response);
        }

        /// <summary>
        /// Delete notification
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, Charity, User
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + ","
                        + AuthConstant.RoleCharity + ","
                        + AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteNotification(string id)
        {
            var response = await _notificationService.DeleteNotificationAsync(id);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Create notification
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin + ","
                        + AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateNotification(NotificationCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _notificationService.CreateNotificationAsync(request);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return Ok(response);
        }
    }
}