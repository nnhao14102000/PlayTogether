using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Services.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AuthController :BaseController
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
        /// <returns>Return token if login successfully</returns>
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
        /// Login google for player
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Return token if login successfully</returns>
        [HttpPost, Route("login-google-player")]
        public async Task<ActionResult<AuthResultDto>> PlayerLoginGoogleAsync(GoogleLoginDto loginDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.LoginPlayerByGoogle(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Login google for hirer
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Return login if login successfully</returns>
        [HttpPost, Route("login-google-hirer")]
        public async Task<ActionResult<AuthResultDto>> HirerLoginGoogleAsync(GoogleLoginDto loginDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.LoginHirerByGoogle(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for admin
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-admin")]
        public async Task<ActionResult<AuthResultDto>> AdminRegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.RegisterAdminAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for charity
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-charity")]
        public async Task<ActionResult<AuthResultDto>> CharityRegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.RegisterCharityAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for player
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-player")]
        public async Task<ActionResult<AuthResultDto>> PlayerRegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.RegisterPlayerAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for hirer
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-hirer")]
        public async Task<ActionResult<AuthResultDto>> HirerRegisterAsync(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _authService.RegisterHirerAsync(registerDto);
            return Ok(response);
        }
    }
}