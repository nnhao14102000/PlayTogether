using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
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
        // private readonly INotificationService _notificationService;

        // public NotificationController(INotificationService notificationService)
        // {
        //     _notificationService = notificationService;
        // }

        // /// <summary>
        // /// Get all notifications 
        // /// </summary>
        // /// <param name="param"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin, Charity, Hirer, Player
        // /// </remarks>
        // [HttpGet]
        // [Authorize(Roles = AuthConstant.RoleAdmin + "," 
        //                 + AuthConstant.RoleCharity + "," 
        //                 + AuthConstant.RoleHirer + "," 
        //                 + AuthConstant.RolePlayer)]
        // public async Task<ActionResult<PagedResult<NotificationGetAllResponse>>> GetAllNotifications(
        //     [FromQuery] NotificationParameters param)
        // {
        //     var response = await _notificationService.GetAllNotificationsAsync(HttpContext.User, param);
        //     var metaData = new {
        //         response.TotalCount,
        //         response.PageSize,
        //         response.CurrentPage,
        //         response.HasNext,
        //         response.HasPrevious
        //     };

        //     Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

        //     return response is not null ? Ok(response) : NotFound();
        // }

        // /// <summary>
        // /// Get notification by Id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin, Charity, Hirer, Player
        // /// </remarks>
        // [HttpGet("{id}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin + "," 
        //                 + AuthConstant.RoleCharity + "," 
        //                 + AuthConstant.RoleHirer + "," 
        //                 + AuthConstant.RolePlayer)]
        // public async Task<ActionResult<NotificationGetDetailResponse>> GetNotificationById(string id){
        //     var response = await _notificationService.GetNotificationByIdAsync(id);
        //     return response is not null ? Ok(response) : NotFound();
        // }

        // /// <summary>
        // /// Delete notification
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin, Charity, Hirer, Player
        // /// </remarks>
        // [HttpDelete("{id}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin + "," 
        //                 + AuthConstant.RoleCharity + "," 
        //                 + AuthConstant.RoleHirer + "," 
        //                 + AuthConstant.RolePlayer)]
        // public async Task<ActionResult> DeleteNotification(string id){
        //     var response = await _notificationService.DeleteNotificationAsync(id);
        //     return response ? NoContent() : NotFound();
        // }
    }
}