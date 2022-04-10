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
        public async Task<ActionResult> GetAllGameTypes(
            [FromQuery] GameTypeParameter param)
        {
            var response = await _gameTypeService.GetAllGameTypesAsync(param);

            if(!response.IsSuccess){
                if (response.Error.Code == 400){
                    return BadRequest(response);
                }else if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));
            return Ok(response);
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
        public async Task<ActionResult> GetGameTypeById(string gameTypeId)
        {
            var response = await _gameTypeService.GetGameTypeByIdAsync(gameTypeId);
            if(!response.IsSuccess){
                if (response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return Ok(response);
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
            if(!response.IsSuccess){
                if (response.Error.Code == 400){
                    return BadRequest(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return CreatedAtRoute(nameof(GetGameTypeById), new { gameTypeId = response.Content.Id }, response);
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
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else if (response.Error.Code == 400){
                    return BadRequest(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return NoContent();
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