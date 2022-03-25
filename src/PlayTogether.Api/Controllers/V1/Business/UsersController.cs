using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Business.UnActiveBalance;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IHobbyService _hobbyService;
        private readonly IGameOfUserService _gameOfUserService;
        private readonly IOrderService _orderService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUnActiveBalanceService _unActiveBalanceService;

        public UsersController(
            IAppUserService appUserService,
            IHobbyService hobbyService,
            IGameOfUserService gameOfUserService,
            IOrderService orderService,
            ITransactionHistoryService transactionHistoryService,
            IUnActiveBalanceService unActiveBalanceService)
        {
            _appUserService = appUserService;
            _hobbyService = hobbyService;
            _gameOfUserService = gameOfUserService;
            _orderService = orderService;
            _transactionHistoryService = transactionHistoryService;
            _unActiveBalanceService = unActiveBalanceService;
        }

        /*
        *================================================
        *                                              ||
        * UPDATE USER INFO APIs SECTION                ||
        *                                              ||
        *================================================
        */
        

        /// <summary>
        /// Update personal info in profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("personal")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> UpdatePersonalInfo(UserPersonalInfoUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.UpdatePersonalInfoAsync(HttpContext.User, request);
            return response ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Update personal service info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("service")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> UpdateUserServiceInfo(UserInfoForIsPlayerUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.UpdateUserServiceInfoAsync(HttpContext.User, request);
            return response ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Update to enable or disable IsPlayer State
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("player")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> ChangeIsPlayer(UserIsPlayerChangeRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.ChangeIsPlayerAsync(HttpContext.User, request);
            return response ? NoContent() : BadRequest();
        }

        /*
        *================================================
        *                                              ||
        * GET USER INFO APIs SECTION                   ||
        *                                              ||
        *================================================
        */
        /// <summary>
        /// Get own personal information
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("personal")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PersonalInfoResponse>> GetPersonalInfo()
        {
            var response = await _appUserService.GetPersonalInfoByIdentityIdAsync(HttpContext.User);
            return response is not null ? Ok(response) : Unauthorized();
        }

        /// <summary>
        /// Get specific user all hobbies
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("{userId}/hobbies")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<HobbiesGetAllResponse>>> GetAllHobbies(
            string userId,
            [FromQuery] HobbyParameters param)
        {
            var response = await _hobbyService.GetAllHobbiesAsync(userId, param);

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
        /// Get a specific user service info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("service/{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<UserGetServiceInfoResponse>> GetUserServiceInfoById(string userId)
        {
            var response = await _appUserService.GetUserServiceInfoByIdAsync(userId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get a specific user info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User, Admin
        /// </remarks>
        [HttpGet, Route("{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser + "," + AuthConstant.RoleAdmin)]
        public async Task<ActionResult<UserGetBasicInfoResponse>> GetUserBasicInfoById(string userId)
        {
            var response = await _appUserService.GetUserBasicInfoByIdAsync(userId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get all games of a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("{userId}/games")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<IEnumerable<GamesOfUserResponse>>> GetAllGameOfUser(string userId)
        {
            var response = await _gameOfUserService.GetAllGameOfUserAsync(userId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get all users / Search users
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<UserSearchResponse>>> GetAllUser([FromQuery] UserParameters param){
            var response = await _appUserService.GetAllUsersAsync(HttpContext.User, param);

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

        /*
        *================================================
        *                                              ||
        * ORDER APIs SECTION                           ||
        *                                              ||
        *================================================
        */

        /// <summary>
        /// Create a Order request
        /// </summary>
        /// <param name="toUserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost("orders/{toUserId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<OrderGetResponse>> CreateOrder(string toUserId, OrderCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _orderService.CreateOrderAsync(HttpContext.User, toUserId, request);

            return response is null 
                ? BadRequest() 
                : CreatedAtRoute(nameof(OrdersController.GetOrderById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Get all Orders (from create User)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("orders")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<IEnumerable<OrderGetResponse>>> GetAllOrderForHirer(
            [FromQuery] UserOrderParameter param)
        {
            var response = await _orderService.GetAllOrdersAsync(HttpContext.User, param);

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
        /// Cancel order request
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Hirer
        /// </remarks>
        [HttpPut("orders/cancel/{orderId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CancelOrderRequest(string orderId)
        {
            var response = await _orderService.CancelOrderAsync(orderId, HttpContext.User);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Get all Orders (for User receive order)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("orders/requests")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<IEnumerable<OrderGetResponse>>> GetAllOrderForPlayer(
            [FromQuery] UserOrderParameter param)
        {
            var response = await _orderService.GetAllOrderRequestsAsync(HttpContext.User, param);

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
        /// Process the Order Request
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut("orders/{orderId}/process")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> ProcessOrderRequest(string orderId, OrderProcessByPlayerRequest request)
        {
            var response = await _orderService.ProcessOrderAsync(orderId, HttpContext.User, request);
            return response ? NoContent() : NotFound();
        }
        
        /*
        *================================================
        *                                              ||
        * Balance APIs SECTION                         ||
        *                                              ||
        *================================================
        */

        /// <summary>
        /// Get all Transaction
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("transactions")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<TransactionHistoryResponse>> GetAllTransaction(
            [FromQuery] TransactionParameters param)
        {
            var response = await _transactionHistoryService.GetAllTransactionHistoriesAsync(HttpContext.User, param);

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
        /// Get all un active money
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("un-active-balance")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<UnActiveBalanceResponse>> GetAllUnActiveBalance(
            [FromQuery] UnActiveBalanceParameters param)
        {
            var response = await _unActiveBalanceService.GetAllUnActiveBalancesAsync(HttpContext.User, param);

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
        /// Check to active money in un active balance
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("un-active-balance")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CheckToActiveBalance()
        {
            var response = await _unActiveBalanceService.ActiveMoneyAsync(HttpContext.User);
            return response ? NoContent() : NotFound();
        }
    }
}