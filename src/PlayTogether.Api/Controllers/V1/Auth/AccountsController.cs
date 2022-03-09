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
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
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

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Hirer, Player
        /// </remarks>
        [HttpPut, Route("logout")]
        [Authorize(Roles = AuthConstant.RoleHirer + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<AuthResult>> LogoutUser()
        {
            var response = await _accountService.LogoutAsync(HttpContext.User);
            return Ok(response);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Hirer, Player, Charity, Admin
        /// </remarks>
        [HttpPut, Route("change-password")]
        [Authorize(Roles = AuthConstant.RoleHirer
                           + ","
                           + AuthConstant.RolePlayer
                           + ","
                           + AuthConstant.RoleCharity
                           + ","
                           + AuthConstant.RoleAdmin)]
        public async Task<ActionResult<AuthResult>> ChangePassword (ChangePasswordRequest request) {
            var response = await _accountService.ChangePasswordAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Reset password for admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("reset-password-admin")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<AuthResult>> ResetPasswordAdmin(ResetPasswordAdminRequest request){
            var response = await _accountService.ResetPasswordAdminAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Reset password token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Hirer, Player, Charity
        /// </remarks>
        [HttpPut, Route("reset-password-token")]
        [Authorize(Roles = AuthConstant.RoleHirer
                           + ","
                           + AuthConstant.RolePlayer
                           + ","
                           + AuthConstant.RoleCharity)]
        public async Task<ActionResult<AuthResult>> ResetPasswordToken(ResetPasswordTokenRequest request){
            var response = await _accountService.ResetPasswordTokenAsync(request);
            return Ok(response);
        }
        
        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Hirer, Player, Charity
        /// </remarks>
        [HttpPut, Route("reset-password")]
        [Authorize(Roles = AuthConstant.RoleHirer
                           + ","
                           + AuthConstant.RolePlayer
                           + ","
                           + AuthConstant.RoleCharity)]
        public async Task<ActionResult<AuthResult>> ResetPassword(ResetPasswordRequest request){
            var response = await _accountService.ResetPasswordAsync(request);
            return Ok(response);
        }
    }
}