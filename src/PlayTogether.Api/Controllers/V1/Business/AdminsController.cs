using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class AdminsController : BaseController
    {
        private readonly IAdminService _adminService;
        private readonly IHirerService _hirerService;
        private readonly IOrderService _orderService;

        public AdminsController(
            IAdminService adminService,
            IHirerService hirerService,
            IOrderService orderService)
        {
            _adminService = adminService;
            _hirerService = hirerService;
            _orderService = orderService;
        }

        /// <summary>
        /// Get all Admins
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<PagedResult<AdminResponse>>> GetAllAdmins(
            [FromQuery] AdminParameters param
        )
        {
            var response = await _adminService.GetAllAdminsAsync(param);

            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get all orders of specific user (Hirer Or Player)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet("{userId}/orders")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<PagedResult<OrderGetResponse>>> GetAllOrderOfUserByAdmin(
            string userId,
            [FromQuery] AdminOrderParameters param)
        {
            var response = await _orderService.GetAllOrderByUserIdForAdminAsync(userId, param);
            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get Order in details by Order Id by Admin
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet("users/orders/{orderId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<OrderGetResponse>> GetOrderByIdForAdmin(string orderId)
        {
            var response = await _orderService.GetOrderByIdInDetailForAdminAsync(orderId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Active or Disable (1 day) a hirer account
        /// </summary>
        /// <param name="hirerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("hirer-status/{hirerId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateHirerStatus(string hirerId, HirerStatusUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _hirerService.UpdateHirerStatusForAdminAsync(hirerId, request);
            return response ? NoContent() : NotFound();
        }

    }
}
