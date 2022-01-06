using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class HirersController : ControllerBase
    {
        /// <summary>
        /// Test login by admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<IActionResult> Test()
        {
            return Ok("Test Admin success");
        }
    }
}