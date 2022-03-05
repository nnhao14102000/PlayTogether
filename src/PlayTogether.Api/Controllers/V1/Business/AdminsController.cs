using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
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

        public AdminsController(IAdminService adminService, IHirerService hirerService)
        {
            _adminService = adminService;
            _hirerService = hirerService;
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
            var response = await _adminService.GetAllAdminsAsync(param).ConfigureAwait(false);

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
