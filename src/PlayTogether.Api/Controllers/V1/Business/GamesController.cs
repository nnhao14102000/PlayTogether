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
        /// Roles Access: Admin, Player, Hirer
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer + "," + AuthConstant.RoleHirer)]
        public async Task<ActionResult<PagedResult<GameGetAllResponse>>> GetAllGames(
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
        /// Roles Access: Admin, Player
        /// </remarks>
        [HttpGet("{gameId}/ranks")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<IEnumerable<RankGetByIdResponse>>> GetAllRanksInGame(string gameId)
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
        public async Task<ActionResult<RankCreateResponse>> CreateRank(string gameId, RankCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _rankService.CreateRankAsync(gameId, request);
            
            return response is null ? BadRequest() : CreatedAtRoute(nameof(RanksController.GetRankById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Get Game by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, Player, Hirer
        /// </remarks>
        [HttpGet("{id}", Name = "GetGameById")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer + "," + AuthConstant.RoleHirer)]
        public async Task<ActionResult<GameGetByIdResponse>> GetGameById(string id)
        {
            var response = await _gameService.GetGameByIdAsync(id);
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
        public async Task<ActionResult<GameCreateResponse>> CreateGame(GameCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameService.CreateGameAsync(request);
            return response is null ? BadRequest() : CreatedAtRoute(nameof(GetGameById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Update Game
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateGame(string id, GameUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _gameService.UpdateGameAsync(id, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a Game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteGame(string id)
        {
            var response = await _gameService.DeleteGameAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}