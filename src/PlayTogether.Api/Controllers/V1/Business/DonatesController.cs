using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class DonatesController : BaseController
    {
        private readonly IDonateService _donateService;
        public DonatesController(IDonateService donateService)
        {
            _donateService = donateService;
        }

        /// <summary>
        /// Get all donates
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleCharity + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllDonates([FromQuery] DonateParameters param)
        {
            var response = await _donateService.GetAllDonatesAsync(HttpContext.User, param);
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
        /// Get donate by Id
        /// </summary>
        /// <param name="donateId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity, User
        /// </remarks>
        [HttpGet, Route("{donateId}")]
        [Authorize(Roles = AuthConstant.RoleCharity + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetDonateById(string donateId)
        {
            var response = await _donateService.GetDonateByIdAsync(donateId);
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