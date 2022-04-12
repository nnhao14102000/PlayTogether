using System.Collections.Generic;
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
        /// Create game of user / skill
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateGameOfUser(
            GameOfUserCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var response = await _gameOfUserService.CreateGameOfUserAsync(HttpContext.User, request);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }

            return CreatedAtRoute(nameof(GetGameOfUserById), new { gameOfUserId = response.Content.Id }, response);
        }

        /// <summary>
        /// Create multi games of user / skills
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost, Route("multi-games")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateMultiGamesOfUser(
            List<GameOfUserCreateRequest> requests)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var response = await _gameOfUserService.CreateMultiGameOfUserAsync(HttpContext.User, requests);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }

            return Ok(response);
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
        public async Task<ActionResult> GetGameOfUserById(string gameOfUserId)
        {
            var response = await _gameOfUserService.GetGameOfUserByIdAsync(gameOfUserId);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return Ok(response);
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
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return NoContent();
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
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return NoContent();
        }
    }
}