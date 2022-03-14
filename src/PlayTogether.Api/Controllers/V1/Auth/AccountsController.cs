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
    public class AccountsController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Login User Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-user")]
        public async Task<ActionResult<AuthResult>> LoginUser(LoginRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginUserAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Login Admin Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-admin")]
        public async Task<ActionResult<AuthResult>> LoginAdmin(LoginRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginAdminAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Login Charity Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-charity")]
        public async Task<ActionResult<AuthResult>> LoginCharity(LoginRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginCharityAsync(request);
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
        /// Login with google
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("login-google")]
        public async Task<ActionResult<AuthResult>> LoginWithGoogle(GoogleLoginRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.LoginUserByGoogleAsync(request);
            return Ok(response);
        }


        /// <summary>
        /// Register admin account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("register-admin")]
        //[Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<AuthResult>> AdminRegister(RegisterAdminInfoRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterAdminAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Register charity account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost, Route("register-charity")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<AuthResult>> CharityRegister(RegisterCharityInfoRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterCharityAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Register user account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("register-user")]
        public async Task<ActionResult<AuthResult>> UserRegister(RegisterUserInfoRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterUserAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Register for multi player
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        [HttpPost, Route("register-multi-player")]
        public async Task<ActionResult> MultiUserIsPlayerRegister(List<RegisterUserInfoRequest> requests)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterMultiUserIsPlayerAsync(requests);
            return response is true ? Ok() : BadRequest();
        }

        /// <summary>
        /// Register for multi normal user
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        [HttpPost, Route("register-multi-user")]
        public async Task<ActionResult> MultiUserRegister(List<RegisterUserInfoRequest> requests)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _accountService.RegisterMultiUserAsync(requests);
            return response is true ? Ok() : BadRequest();
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("logout")]
        [Authorize(Roles = AuthConstant.RoleUser)]
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
        /// Roles Access: User, Charity, Admin
        /// </remarks>
        [HttpPut, Route("change-password")]
        [Authorize(Roles = AuthConstant.RoleUser
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
        /// Roles Access: User, Charity
        /// </remarks>
        [HttpPut, Route("reset-password-token")]
        [Authorize(Roles = AuthConstant.RoleUser
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
        /// Roles Access: User, Charity
        /// </remarks>
        [HttpPut, Route("reset-password")]
        [Authorize(Roles = AuthConstant.RoleUser
                           + ","
                           + AuthConstant.RoleCharity)]
        public async Task<ActionResult<AuthResult>> ResetPassword(ResetPasswordRequest request){
            var response = await _accountService.ResetPasswordAsync(request);
            return Ok(response);
        }
    }
}