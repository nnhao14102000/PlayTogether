using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfPlayer;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class GameOfPlayersController : BaseController
    {
        private readonly IGameOfPlayerService _gameOfPlayerService;

        public GameOfPlayersController(IGameOfPlayerService gameOfPlayerService)
        {
            _gameOfPlayerService = gameOfPlayerService;
        }

        /// <summary>
        /// Get Game of Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetGameOfPlayerById")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<GameOfPlayerGetByIdResponse>> GetGameOfPlayerById(string id)
        {
            var response = await _gameOfPlayerService.GetGameOfPlayerByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update Game of Player
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult> UpdateGameOfPlayer(string id, GameOfPlayerUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameOfPlayerService.UpdateGameOfPlayerAsync(id, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete Game of Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult> DeleteGameOfPlayer(string id)
        {
            var response = await _gameOfPlayerService.DeleteGameOfPlayerAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}