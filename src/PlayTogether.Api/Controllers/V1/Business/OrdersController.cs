using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Get Order by Order Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("{id}", Name = "GetOrderById")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<OrderGetResponse>> GetOrderById(string id)
        {
            var response = await _orderService.GetOrderByIdAsync(HttpContext.User, id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Finish Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut("finish/{id}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> FinishOrder(string id)
        {
            var response = await _orderService.FinishOrderAsync(id);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Finish Soon Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut("finish-soon/{id}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> FinishSoonOrder(string id, FinishSoonRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _orderService.FinishOrderSoonAsync(id, HttpContext.User, request);
            return response ? NoContent() : NotFound();
        }
        

        /// <summary>
        /// Get Order in details by Order Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("detail/{orderId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + AuthConstant.RoleUser)]
        public async Task<ActionResult<OrderGetDetailResponse>> GetOrderInDetailById(string orderId)
        {
            var response = await _orderService.GetOrderByIdInDetailAsync(orderId);
            return response is not null ? Ok(response) : NotFound();
        }
        
    }
}