using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Services.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Token if login successfully</returns>
        [HttpPost, Route("login")]
        public async Task<ActionResult<AuthResultDto>> LoginAsync(LoginDto loginDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.LoginUserAsync(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for admin
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("admin-register")]
        public async Task<ActionResult<AuthResultDto>> AdminRegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.RegisterAdminAsync(registerDto);
            return Ok(response);
        }
    }
}