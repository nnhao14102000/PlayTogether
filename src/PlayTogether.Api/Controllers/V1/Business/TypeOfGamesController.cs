using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class TypeOfGamesController : BaseController
    {
        private readonly ITypeOfGameService _typeOfGameService;

        public TypeOfGamesController(ITypeOfGameService typeOfGameService)
        {
            _typeOfGameService = typeOfGameService;
        }

        /// <summary>
        /// Get Type of Game by Id for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetTypeOfGameById")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<TypeOfGameGetByIdResponse>> GetTypeOfGameById(string id)
        {
            var response = await _typeOfGameService.GetTypeOfGameByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add Type of Game for Admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<TypeOfGameGetByIdResponse>> CreateGame(
            TypeOfGameCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _typeOfGameService.CreateTypeOfGameAsync(request);
            return CreatedAtRoute(nameof(GetTypeOfGameById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Delete Type Of Game for Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteGame(string id)
        {
            var response = await _typeOfGameService.DeleteTypeOfGameAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}