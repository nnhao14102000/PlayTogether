using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/types-of-game")]
    public class TypeOfGamesController : BaseController
    {
        private readonly ITypeOfGameService _typeOfGameService;

        public TypeOfGamesController(ITypeOfGameService typeOfGameService)
        {
            _typeOfGameService = typeOfGameService;
        }

        /// <summary>
        /// Get Type of Game by Id
        /// </summary>
        /// <param name="typeOfGameId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{typeOfGameId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult<TypeOfGameGetByIdResponse>> GetTypeOfGameById(string typeOfGameId)
        {
            var response = await _typeOfGameService.GetTypeOfGameByIdAsync(typeOfGameId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add Type of Game
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> CreateTypeOfGame(
            TypeOfGameCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _typeOfGameService.CreateTypeOfGameAsync(request);
            return response ? Ok() : BadRequest();
        }

        /// <summary>
        /// Delete Type Of Game
        /// </summary>
        /// <param name="typeOfGameId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{typeOfGameId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteTypeOfGame(string typeOfGameId)
        {
            var response = await _typeOfGameService.DeleteTypeOfGameAsync(typeOfGameId);
            return response ? NoContent() : NotFound();
        }
    }
}