using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class MusicsController : BaseController
    {
        private readonly IMusicService _musicService;

        public MusicsController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        /// <summary>
        /// Get all Musics for Admin, Player
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<PagedResult<MusicGetByIdResponse>>> GetAllMusics(
            [FromQuery] MusicParameter param)
        {
            var response = await _musicService.GetAllMusicsAsync(param);

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
        /// Get Music by Id for Admin, Player
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMusicById")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<MusicGetByIdResponse>> GetMusicById(string id)
        {
            var response = await _musicService.GetMusicByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add Music for Admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<MusicGetByIdResponse>> CreateMusic(MusicCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _musicService.CreateMusicAsync(request);
            return CreatedAtRoute(nameof(GetMusicById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Update Music for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut, Route("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateMusic(string id, MusicUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _musicService.UpdateMusicAsync(id, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a Music for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteMusic(string id)
        {
            var response = await _musicService.DeleteMusicAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}