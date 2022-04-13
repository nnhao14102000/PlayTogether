using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Business.Donate;
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
        private readonly IDonateService _donateService;
        private readonly IRecommendService _recommendService;
        private readonly IDatingService _datingService;

        public UsersController(
            IAppUserService appUserService,
            IHobbyService hobbyService,
            IGameOfUserService gameOfUserService,
            IOrderService orderService,
            ITransactionHistoryService transactionHistoryService,
            IUnActiveBalanceService unActiveBalanceService,
            IDonateService donateService,
            IRecommendService recommendService,
            IDatingService datingService)
        {
            _appUserService = appUserService;
            _hobbyService = hobbyService;
            _gameOfUserService = gameOfUserService;
            _orderService = orderService;
            _transactionHistoryService = transactionHistoryService;
            _unActiveBalanceService = unActiveBalanceService;
            _donateService = donateService;
            _recommendService = recommendService;
            _datingService = datingService;
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
        public async Task<ActionResult> GetPersonalInfo()
        {
            var response = await _appUserService.GetPersonalInfoByIdentityIdAsync(HttpContext.User);
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
        public async Task<ActionResult> GetAllHobbies(
            string userId,
            [FromQuery] HobbyParameters param)
        {
            var response = await _hobbyService.GetAllHobbiesAsync(userId, param);

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
        /// Get a specific user service info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("service/{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> GetUserServiceInfoById(string userId)
        {
            var response = await _appUserService.GetUserServiceInfoByIdAsync(userId);
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
        /// Get a specific user info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User, Admin
        /// </remarks>
        [HttpGet, Route("{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser + "," + AuthConstant.RoleAdmin)]
        public async Task<ActionResult> GetUserBasicInfoById(string userId)
        {
            var response = await _appUserService.GetUserBasicInfoByIdAsync(userId);
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
        /// Get all games of a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("{userId}/games")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllGameOfUser(string userId, [FromQuery] GameOfUserParameters param)
        {
            var response = await _gameOfUserService.GetAllGameOfUserAsync(userId, param);
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
        /// Get all users / Search users
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllUser([FromQuery] UserParameters param)
        {
            var response = await _appUserService.GetAllUsersAsync(HttpContext.User, param);

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
        /// Get all dating of a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("{userId}/datings")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllDatings(string userId, [FromQuery] DatingParameters param)
        {
            var response = await _datingService.GetAllDatingsOfUserAsync(userId, param);
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
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }

            return CreatedAtRoute(nameof(OrdersController.GetOrderById), new { id = response.Content.Id }, response);
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
        public async Task<ActionResult<PagedResult<OrderGetResponse>>> GetAllOrderForHirer(
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
        /// Get all Orders (for User receive order)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("orders/requests")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<OrderGetResponse>>> GetAllOrderForPlayer(
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
        public async Task<ActionResult<PagedResult<TransactionHistoryResponse>>> GetAllTransaction(
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
        public async Task<ActionResult<PagedResult<UnActiveBalanceResponse>>> GetAllUnActiveBalance(
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

        /*
        *================================================
        *                                              ||
        * Donate APIs SECTION                          ||
        *                                              ||
        *================================================
        */

        /// <summary>
        /// Make donate
        /// </summary>
        /// <param name="charityId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost, Route("donates/{charityId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> Donate(string charityId, DonateCreateRequest request)
        {
            var response = await _donateService.CreateDonateAsync(HttpContext.User, charityId, request);
            return response ? Ok() : NotFound();
        }


        /*
        *================================================
        *                                              ||
        * Active APIs SECTION                          ||
        *                                              ||
        *================================================
        */
        /// <summary>
        /// Get disable info
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("disable")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<DisableUserResponse>> GetDisableInfo()
        {
            var response = await _appUserService.GetDisableInfoAsync(HttpContext.User);
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
        /// Active account
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("active")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> ActiveUser()
        {
            var response = await _appUserService.ActiveUserAsync(HttpContext.User);
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

        /*
        *================================================
        *                                              ||
        * ML APIs SECTION                              ||
        *                                              ||
        *================================================
        */


    }
}