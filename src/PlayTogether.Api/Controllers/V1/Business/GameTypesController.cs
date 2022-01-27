using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class GameTypesController : BaseController
    {
        private readonly IGameTypeService _gameTypeService;

        public GameTypesController(IGameTypeService gameTypeService)
        {
            _gameTypeService = gameTypeService;
        }

        /// <summary>
        /// Get all Game Type for Admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
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
        /// Get Game Type by Id for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetGameTypeById")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<GameTypeGetByIdResponse>> GetGameTypeById(string id)
        {
            var response = await _gameTypeService.GetGameTypeByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add New a Game Type for Admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<GameTypeCreateResponse>> CreateGameType(GameTypeCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameTypeService.CreateGameTypeAsync(request);
            return CreatedAtRoute(nameof(GetGameTypeById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Update Game Type for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateGameType(string id, GameTypeUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameTypeService.UpdateGameTypeAsync(id, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a Game Type for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteGameType(string id)
        {
            var response = await _gameTypeService.DeleteGameTypeAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}