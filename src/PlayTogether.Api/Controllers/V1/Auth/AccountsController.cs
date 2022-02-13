using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outgoing.Auth;
using PlayTogether.Core.Interfaces.Services.Auth;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Auth
{
    [ApiVersion("1.0")]
    public class AccountsController :BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Return token if login successfully</returns>
        [HttpPost, Route("login")]
        public async Task<ActionResult<AuthResult>> Login(LoginRequest loginDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginUserAsync(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Check is email exist in database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet, Route("check-exist-email")]
        public async Task<bool> CheckEmailExist(string email)
        {
            return email == null ? false : await _accountService.CheckExistEmailAsync(email);
        }

        /// <summary>
        /// Login with google for player
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Return token if login successfully</returns>
        [HttpPost, Route("login-google-player")]
        public async Task<ActionResult<AuthResult>> PlayerLoginGoogle(GoogleLoginRequest loginDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginPlayerByGoogleAsync(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Login with google for hirer
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns>Return token if login successfully</returns>
        [HttpPost, Route("login-google-hirer")]
        public async Task<ActionResult<AuthResult>> HirerLoginGoogle(GoogleLoginRequest loginDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginHirerByGoogleAsync(loginDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for admin
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-admin")]
        //[Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<AuthResult>> AdminRegister(RegisterAdminInfoRequest registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterAdminAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for charity
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-charity")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<AuthResult>> CharityRegister(RegisterCharityInfoRequest registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterCharityAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for player
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-player")]
        public async Task<ActionResult<AuthResult>> PlayerRegister(RegisterUserInfoRequest registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterPlayerAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for hirer
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost, Route("register-hirer")]
        public async Task<ActionResult<AuthResult>> HirerRegister(RegisterUserInfoRequest registerDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterHirerAsync(registerDto);
            return Ok(response);
        }

        /// <summary>
        /// Register for multi player
        /// </summary>
        /// <param name="registerDtos"></param>
        /// <returns></returns>
        [HttpPost, Route("register-multi-player")]
        public async Task<ActionResult> MultiPlayerRegister(List<RegisterUserInfoRequest> registerDtos)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterMultiPlayerAsync(registerDtos);
            return response is true ? Ok() : BadRequest();
        }

        /// <summary>
        /// Register for multi hirer
        /// </summary>
        /// <param name="registerDtos"></param>
        /// <returns></returns>
        [HttpPost, Route("register-multi-hirer")]
        public async Task<ActionResult> MultiHirerRegister(List<RegisterUserInfoRequest> registerDtos)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterMultiHirerAsync(registerDtos);
            return response is true ? Ok() : BadRequest();
        }
    }
}