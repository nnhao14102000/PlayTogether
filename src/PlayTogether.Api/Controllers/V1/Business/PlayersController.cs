using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business.Player;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class PlayersController : BaseController
    {
        private readonly IPlayerService _playerService;
        
        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// Get all Players for Hirer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult<PagedResult<GetAllPlayerResponseForHirer>>> GetAllPlayers(
            [FromQuery] PlayerParameters param)
        {
            var response = await _playerService.GetAllPlayersForHirerAsync(param).ConfigureAwait(false);

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
        public async Task<ActionResult<GetPlayerProfileResponse>> GetPlayerProfile()
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
        public async Task<ActionResult<GetPlayerByIdResponseForPlayer>> GetPlayerById(string id)
        {
            var response = await _playerService.GetPlayerByIdForPlayerAsync(id);
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
        public async Task<ActionResult> UpdatePlayerInfomation(string id, UpdatePlayerInfoRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _playerService.UpdatePlayerInformationAsync(id, request);
            return response ? NoContent() : NotFound();
        }
    }
}
