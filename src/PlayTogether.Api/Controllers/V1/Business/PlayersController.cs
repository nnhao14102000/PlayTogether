using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Interfaces.Services.Business.Player;
using System.Collections.Generic;
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
        /// Get all Players
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerResponseDto>>> GetAllPlayers()
        {
            var response = await _playerService.GetAllPlayerAsync().ConfigureAwait(false);
            return response != null ? Ok(response) : NotFound();
        }
    }
}
