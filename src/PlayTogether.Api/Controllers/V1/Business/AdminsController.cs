using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Interfaces.Services.Business.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class AdminsController : BaseController
    {
        private readonly IAdminService _adminService;
        public AdminsController (IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Get all admins
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminResponseDto>>> GetAllAdmins()
        {
            var response = await _adminService.GetAllAdminAsync().ConfigureAwait(false);
            return response != null ? Ok(response) : NotFound();
        }
    }
}
