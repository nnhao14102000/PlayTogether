using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class HirersController : BaseController
    {
        private readonly IHirerService _hirerService;

        public HirersController(IHirerService HirerService)
        {
            _hirerService = HirerService;
        }

        /// <summary>
        /// Get all Hirer for Admin
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<IEnumerable<HirerGetAllResponseForAdmin>>> GetAllHirers(
            [FromQuery] HirerParameters param)
        {
            var response = await _hirerService.GetAllHirersForAdminAsync(param).ConfigureAwait(false);

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
        /// Get Hirer Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("profile")]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult<HirerGetProfileResponse>> GetHirerProfile()
        {
            var response = await _hirerService.GetHirerProfileByIdentityIdAsync(HttpContext.User);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get Hirer by Id for Hirer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult<HirerGetByIdResponseForHirer>> GetHirerById(string id)
        {
            var response = await _hirerService.GetHirerByIdForHirerAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update Hirer Information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult> UpdateHirerInfomation(string id, HirerInfoUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _hirerService.UpdateHirerInformationAsync(id, request);
            return response ? NoContent() : NotFound();
        }

    }
}
