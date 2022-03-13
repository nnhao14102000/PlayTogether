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
        // private readonly IReportService _reportService;

        // public ReportsController(IReportService reportService)
        // {
        //     _reportService = reportService;
        // }

        // /// <summary>
        // /// Create report
        // /// </summary>
        // /// <param name="orderId"></param>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Player
        // /// </remarks>
        // [HttpPost("{orderId}")]
        // [Authorize(Roles = AuthConstant.RolePlayer)]
        // public async Task<ActionResult> CreateReport(string orderId, ReportCreateRequest request)
        // {
        //     if (!ModelState.IsValid) {
        //         return BadRequest();
        //     }
        //     var response = await _reportService.CreateReportAsync(orderId, request);
        //     return response ? Ok() : NotFound();
        // }

        // /// <summary>
        // /// Get all reports of a specific hirer
        // /// </summary>
        // /// <param name="param"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Player, Hirer, Admin
        // /// </remarks>
        // [HttpGet("{hirerId}")]
        // [Authorize(Roles = AuthConstant.RoleHirer
        //                    + ","
        //                    + AuthConstant.RolePlayer
        //                    + ","
        //                    + AuthConstant.RoleAdmin)]
        // public async Task<ActionResult<PagedResult<ReportGetResponse>>> GetAllReports(
        //     string hirerId,
        //     [FromQuery] ReportParamters param)
        // {
        //     var response = await _reportService.GetAllReportsAsync(hirerId, param);
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
    }
}