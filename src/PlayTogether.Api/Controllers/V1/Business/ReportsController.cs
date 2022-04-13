using System.Net;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using Newtonsoft.Json;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class ReportsController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Create report
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost("{orderId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateReport(string orderId, ReportCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _reportService.CreateReportAsync(HttpContext.User, orderId, request);
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
        /// Get all report of a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User, Admin
        /// </remarks>
        [HttpGet("{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser
                           + ","
                           + AuthConstant.RoleAdmin)]
        public async Task<ActionResult> GetAllReports(
            string userId,
            [FromQuery] ReportParamters param)
        {
            var response = await _reportService.GetAllReportsAsync(userId, param);
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
    }
}