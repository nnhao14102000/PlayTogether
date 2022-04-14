using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class AdminsController : BaseController
    {
        private readonly IAdminService _adminService;
        // private readonly IHirerService _hirerService;
        private readonly IOrderService _orderService;
        private readonly IReportService _reportService;
        private readonly IAppUserService _appUserService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly ICharityService _charityService;
        private readonly ISystemFeedbackService _systemFeedbackService;
        private readonly IRecommendService _recommendService;
        // private readonly IPlayerService _playerService;

        public AdminsController(
            IAdminService adminService,
        //     IHirerService hirerService,
            IOrderService orderService,
            IReportService reportService,
            IAppUserService appUserService,
            ITransactionHistoryService transactionHistoryService,
            ICharityService charityService,
            ISystemFeedbackService systemFeedbackService,
            IRecommendService recommendService
        //     IPlayerService playerService
        )
        {
            _adminService = adminService;
            //     _hirerService = hirerService;
            _orderService = orderService;
            _reportService = reportService;
            _appUserService = appUserService;
            _transactionHistoryService = transactionHistoryService;
            _charityService = charityService;
            _systemFeedbackService = systemFeedbackService;
            _recommendService = recommendService;
            //     _playerService = playerService;
        }

        /// <summary>
        /// Get all transaction of a User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet("transactions/{userId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<TransactionHistoryResponse>> GetAllTransactionOfUser(string userId,
            [FromQuery] TransactionParameters param)
        {
            var response = await _transactionHistoryService.GetAllTransactionHistoriesForAdminAsync(userId, param);

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


        // /// <summary>
        // /// Get all Admins
        // /// </summary>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpGet]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult<PagedResult<AdminResponse>>> GetAllAdmins(
        //     [FromQuery] AdminParameters param
        // )
        // {
        //     var response = await _adminService.GetAllAdminsAsync(param);

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

        /// <summary>
        /// Get all orders of specific user 
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



        // /// <summary>
        // /// Active or Disable (1 day) a hirer account
        // /// </summary>
        // /// <param name="hirerId"></param>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpPut, Route("hirer-status/{hirerId}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult> UpdateHirerStatus(string hirerId, HirerStatusUpdateRequest request)
        // {
        //     if (!ModelState.IsValid) {
        //         return BadRequest();
        //     }
        //     var response = await _hirerService.UpdateHirerStatusForAdminAsync(hirerId, request);
        //     return response ? NoContent() : NotFound();
        // }

        /// <summary>
        /// Get all reports from all users
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet, Route("reports")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> GetAllReports([FromQuery] ReportAdminParameters param)
        {
            var response = await _reportService.GetAllReportsForAdminAsync(param);
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
        /// Get a report in detail by Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet, Route("reports/{reportId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<ReportInDetailResponse>> GetReportInDetailById(string reportId)
        {
            var response = await _reportService.GetReportInDetailByIdForAdminAsync(reportId);
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
        /// Make approve or not the report
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("reports/{reportId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> ProcessReport(string reportId, ReportCheckRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _reportService.ProcessReportAsync(reportId, request);
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
        /// Get all users
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet, Route("users")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> GetAllUser([FromQuery] AdminUserParameters param)
        {
            var response = await _appUserService.GetAllUsersForAdminAsync(param);

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

        // /// <summary>
        // /// Get player by Id for admin
        // /// </summary>
        // /// <param name="playerId"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpGet, Route("players/{playerId}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult<PlayerGetByIdForAdminResponse>> GetPlayerByIdForAdmin(string playerId)
        // {
        //     var response = await _playerService.GetPlayerByIdForAdminAsync(playerId);
        //     return response is not null ? Ok(response) : NotFound();
        // }

        // /// <summary>
        // /// Active or Disable (1 day) a player account
        // /// </summary>
        // /// <param name="playerId"></param>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpPut, Route("player-status/{playerId}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult> UpdatePlayerStatus(string playerId, PlayerStatusUpdateRequest request)
        // {
        //     if (!ModelState.IsValid) {
        //         return BadRequest();
        //     }
        //     var response = await _playerService.UpdatePlayerStatusForAdminAsync(playerId, request);
        //     return response ? NoContent() : NotFound();
        // }


        /// <summary>
        /// Active or disable charity
        /// </summary>
        /// <param name="charityId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("charities/{charityId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateCharityStatus(string charityId, CharityStatusRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _charityService.ChangeStatusCharityByAdminAsync(charityId, request);
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
        /// Active or disable user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("users/activate/{userId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> ActiveUser(string userId, IsActiveChangeRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.ChangeIsActiveUserForAdminAsync(userId, request);
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
        /// Process feedback
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("feedbacks/{feedbackId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> ProcessFeedback(string feedbackId, ProcessFeedbackRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _systemFeedbackService.ProcessFeedbackAsync(feedbackId, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Get number of Report, DisableUser, SystemFeedback Suggest, New User
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet, Route("dash-board")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<(int, int, int, int)>> AdminStatistic()
        {
            var response = await _adminService.AdminStatisticAsync();
            return response.Item1 >= 0
                   && response.Item2 >= 0
                   && response.Item3 >= 0
                   && response.Item4 >= 0 ? Ok(response) : BadRequest();
        }

        /// <summary>
        /// Train model
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet, Route("train-model")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> TrainModel()
        {
            var response = await _recommendService.TrainModel();
            return response ? Ok() : NoContent();
        }
    }
}
