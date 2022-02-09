﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class PlayersController : BaseController
    {
        private readonly IPlayerService _playerService;
        private readonly IGameOfPlayerService _gameOfPlayerService;

        public PlayersController(
            IPlayerService playerService,
            IGameOfPlayerService gameOfPlayerService)
        {
            _playerService = playerService;
            _gameOfPlayerService = gameOfPlayerService;
        }

        /// <summary>
        /// Create Game of Player
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{playerId}/games")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<GameOfPlayerGetByIdResponse>> CreateGameOfPlayer(
            string playerId,
            GameOfPlayerCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var response = await _gameOfPlayerService.CreateGameOfPlayerAsync(playerId, request);

            return response is null ? BadRequest() : CreatedAtRoute(nameof(GameOfPlayersController.GetGameOfPlayerById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Get all Game of Player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{playerId}/games")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<IEnumerable<GamesInPlayerGetAllResponse>>> GetAllGameOfPlayer(string playerId)
        {
            var response = await _gameOfPlayerService.GetAllGameOfPlayerAsync(playerId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get all Players for Hirer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.1")]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult<PagedResult<PlayerGetAllResponseForHirer>>> GetAllPlayers(
            [FromQuery] PlayerParameters param)
        {
            var response = await _playerService.GetAllPlayersForHirerAsync(param);

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
        /// Get Player Profile
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("profile")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<PlayerGetProfileResponse>> GetPlayerProfile()
        {
            var response = await _playerService.GetPlayerProfileByIdentityIdAsync(HttpContext.User);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get Player by Id for Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<PlayerGetByIdResponseForPlayer>> GetPlayerByIdForPlayer(string id)
        {
            var response = await _playerService.GetPlayerByIdForPlayerAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get Player by Id for Hirer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [HttpGet, Route("{id}")]
        [ApiVersion("1.1")]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult<PlayerGetByIdResponseForHirer>> GetPlayerByIdForHirer(string id)
        {
            var response = await _playerService.GetPlayerByIdForHirerAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update Player Information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult> UpdatePlayerInfomation(string id, PlayerInfoUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _playerService.UpdatePlayerInformationAsync(id, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Get Player Service Info by Id for Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [HttpGet, Route("service-info/{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<PlayerServiceInfoResponseForPlayer>> GetPlayerServiceInfoById(string id)
        {
            var response = await _playerService.GetPlayerServiceInfoByIdForPlayerAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update Player Service Info
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("service-info/{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult> UpdatePlayerInfoService(string id, PlayerServiceInfoUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _playerService.UpdatePlayerServiceInfoAsync(id, request);
            return response ? NoContent() : NotFound();
        }
    }
}
