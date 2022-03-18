using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/games-of-user")]
    public class GameOfUsersController : BaseController
    {
        private readonly IGameOfUserService _gameOfUserService;

        public GameOfUsersController(IGameOfUserService gameOfUserService)
        {
            _gameOfUserService = gameOfUserService;
        }

        /// <summary>
        /// Create game of user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<GameOfUserGetByIdResponse>> CreateGameOfUser(
            GameOfUserCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var response = await _gameOfUserService.CreateGameOfUserAsync(HttpContext.User, request);

            return response is null ? BadRequest() : CreatedAtRoute(nameof(GetGameOfUserById), new { gameOfUserId = response.Id }, response);
        }

        /// <summary>
        /// Get a game of user
        /// </summary>
        /// <param name="gameOfUserId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("{gameOfUserId}", Name = "GetGameOfUserById")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<GameOfUserGetByIdResponse>> GetGameOfUserById(string gameOfUserId)
        {
            var response = await _gameOfUserService.GetGameOfUserByIdAsync(gameOfUserId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update game of user
        /// </summary>
        /// <param name="gameOfUserId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("{gameOfUserId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> UpdateGameOfUser(string gameOfUserId, GameOfUserUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameOfUserService.UpdateGameOfUserAsync(HttpContext.User, gameOfUserId, request);
            return response ? NoContent() : BadRequest();
        }

        /// <summary>
        /// Delete game of user
        /// </summary>
        /// <param name="gameOfUserId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete, Route("{gameOfUserId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteGameOfUser(string gameOfUserId)
        {
            var response = await _gameOfUserService.DeleteGameOfUserAsync(HttpContext.User, gameOfUserId);
            return response ? NoContent() : BadRequest();
        }
    }
}