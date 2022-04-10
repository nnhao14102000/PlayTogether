using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class GamesController : BaseController
    {
        private readonly IGameService _gameService;
        private readonly IRankService _rankService;

        public GamesController(IGameService gameService, IRankService rankService)
        {
            _gameService = gameService;
            _rankService = rankService;
        }

        /// <summary>
        /// Get all Games
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllGames(
            [FromQuery] GameParameter param)
        {
            var response = await _gameService.GetAllGamesAsync(param);

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
        /// Get all Ranks in Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{gameId}/ranks")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllRanksInGame(string gameId)
        {
            var response = await _rankService.GetAllRanksInGameAsync(gameId);
            return response is not null ? Ok(response) : NotFound();
        }


        /// <summary>
        /// Create a Rank in Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost("{gameId}/ranks")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> CreateRank(string gameId, RankCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _rankService.CreateRankAsync(gameId, request);

            return response is null ? BadRequest() : CreatedAtRoute(nameof(RanksController.GetRankById), new { rankId = response.Id }, response);
        }

        /// <summary>
        /// Get Game by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{gameId}", Name = "GetGameById")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetGameById(string gameId)
        {
            var response = await _gameService.GetGameByIdAsync(gameId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add New a Game
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> CreateGame(GameCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameService.CreateGameAsync(request);
            if(!response.IsSuccess){
                if (response.Error.Code == 400){
                    return BadRequest(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return CreatedAtRoute(nameof(GetGameById), new { gameId = response.Content.Id }, response);
        }

        /// <summary>
        /// Update Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("{gameId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateGame(string gameId, GameUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameService.UpdateGameAsync(gameId, request);
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
        /// Delete a Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{gameId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteGame(string gameId)
        {
            var response = await _gameService.DeleteGameAsync(gameId);
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