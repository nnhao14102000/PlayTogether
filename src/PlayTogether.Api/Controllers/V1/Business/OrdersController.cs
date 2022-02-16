using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
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
        /// Get Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetOrderById")]
        [Authorize(Roles = AuthConstant.RoleHirer + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<OrderGetByIdResponse>> GetOrderById(string id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Finish Order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = AuthConstant.RoleHirer + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult> FinishOrder(string id)
        {
            var response = await _orderService.FinishOrderAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}