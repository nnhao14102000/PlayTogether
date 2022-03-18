using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly IAppUserService _appUserService;
        private readonly IHobbyService _hobbyService;
        private readonly IGameOfUserService _gameOfUserService;

        public UsersController(IAppUserService appUserService, IHobbyService hobbyService, IGameOfUserService gameOfUserService)
        {
            _appUserService = appUserService;
            _hobbyService = hobbyService;
            _gameOfUserService = gameOfUserService;
        }

        /// <summary>
        /// Get own personal information
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("personal")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PersonalInfoResponse>> GetPersonalInfo()
        {
            var response = await _appUserService.GetPersonalInfoByIdentityIdAsync(HttpContext.User);
            return response is not null ? Ok(response) : Unauthorized();
        }

        /// <summary>
        /// Get specific user all hobbies
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("{userId}/hobbies")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<HobbiesGetAllResponse>>> GetAllHobbies(
            string userId,
            [FromQuery] HobbyParameters param)
        {
            var response = await _hobbyService.GetAllHobbiesAsync(userId, param);

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
        /// Update personal info in profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("personal")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> UpdatePersonalInfo(UserPersonalInfoUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.UpdatePersonalInfoAsync(HttpContext.User, request);
            return response ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Update personal service info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("service")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> UpdateUserServiceInfo(UserInfoForIsPlayerUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.UpdateUserServiceInfoAsync(HttpContext.User, request);
            return response ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Update to enable or disable IsPlayer State
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("player")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> ChangeIsPlayer(UserIsPlayerChangeRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _appUserService.ChangeIsPlayerAsync(HttpContext.User, request);
            return response ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Get a specific user service info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet, Route("service/{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<UserGetServiceInfoResponse>> GetUserServiceInfoById(string userId)
        {
            var response = await _appUserService.GetUserServiceInfoByIdAsync(userId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get a specific user info
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User, Admin
        /// </remarks>
        [HttpGet, Route("{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser + "," + AuthConstant.RoleAdmin)]
        public async Task<ActionResult<UserGetBasicInfoResponse>> GetUserBasicInfoById(string userId)
        {
            var response = await _appUserService.GetUserBasicInfoByIdAsync(userId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get all games of a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("{userId}/games")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<IEnumerable<GamesOfUserResponse>>> GetAllGameOfUser(string userId)
        {
            var response = await _gameOfUserService.GetAllGameOfUserAsync(userId);
            return response is not null ? Ok(response) : NotFound();
        }

        
    }
}