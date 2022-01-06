using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    public class TestRolesController : BaseController
    {

        [HttpGet("test-admin")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<IActionResult> TestAdmin()
        {
            return Ok("Test admin successfully");
        }

        [HttpGet("test-charity")]
        [Authorize(Roles = AuthConstant.RoleCharity + "," + AuthConstant.RoleAdmin)]
        public async Task<IActionResult> TestCharity()
        {
            return Ok("Test charity successfully");
        }

        [HttpGet("test-player")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<IActionResult> TestPlayer()
        {
            return Ok("Test player successfully");
        }

        [HttpGet("test-hirer")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleHirer)]
        public async Task<IActionResult> TestHirer()
        {
            return Ok("Test hirer successfully");
        }
    }
}
