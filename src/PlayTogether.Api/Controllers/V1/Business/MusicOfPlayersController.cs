using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Business.MusicOfPlayer;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/musics-of-player")]
    public class MusicOfPlayersController : BaseController
    {
        private readonly IMusicOfPlayerService _musicOfPlayerService;
        public MusicOfPlayersController(IMusicOfPlayerService musicOfPlayerService)
        {
            _musicOfPlayerService = musicOfPlayerService;
        }

        /// <summary>
        /// Get Music of Player by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMusicOfPlayerById")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<MusicOfPlayerGetByIdResponse>> GetMusicOfPlayerById(string id)
        {
            var response = await _musicOfPlayerService.GetMusicOfPlayerByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Delete Music of Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult> DeleteMusicOfPlayer(string id)
        {
            var response = await _musicOfPlayerService.DeleteMusicOfPlayerAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}