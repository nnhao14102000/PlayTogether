using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/game-types")]
    public class GameTypesController : BaseController
    {
        private readonly IGameTypeService _gameTypeService;

        public GameTypesController(IGameTypeService gameTypeService)
        {
            _gameTypeService = gameTypeService;
        }

        /// <summary>
        /// Get all Game Types
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<GameTypeGetAllResponse>>> GetAllGameTypes(
            [FromQuery] GameTypeParameter param)
        {
            var response = await _gameTypeService.GetAllGameTypesAsync(param);

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
        /// Get Game Type by Id
        /// </summary>
        /// <param name="gameTypeId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{gameTypeId}", Name = "GetGameTypeById")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult<GameTypeGetByIdResponse>> GetGameTypeById(string gameTypeId)
        {
            var response = await _gameTypeService.GetGameTypeByIdAsync(gameTypeId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add New a Game Type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<GameTypeCreateResponse>> CreateGameType(GameTypeCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameTypeService.CreateGameTypeAsync(request);
            return response is null ? BadRequest() : CreatedAtRoute(nameof(GetGameTypeById), new { gameTypeId = response.Id }, response);
        }

        /// <summary>
        /// Update Game Type
        /// </summary>
        /// <param name="gameTypeId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("{gameTypeId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateGameType(string gameTypeId, GameTypeUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameTypeService.UpdateGameTypeAsync(gameTypeId, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a Game Type
        /// </summary>
        /// <param name="gameTypeId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{gameTypeId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteGameType(string gameTypeId)
        {
            var response = await _gameTypeService.DeleteGameTypeAsync(gameTypeId);
            return response ? NoContent() : NotFound();
        }
    }
}