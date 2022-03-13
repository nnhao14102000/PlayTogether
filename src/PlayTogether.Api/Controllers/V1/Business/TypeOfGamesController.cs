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
        // private readonly ITypeOfGameService _typeOfGameService;

        // public TypeOfGamesController(ITypeOfGameService typeOfGameService)
        // {
        //     _typeOfGameService = typeOfGameService;
        // }

        // /// <summary>
        // /// Get Type of Game by Id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpGet("{id}", Name = "GetTypeOfGameById")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult<TypeOfGameGetByIdResponse>> GetTypeOfGameById(string id)
        // {
        //     var response = await _typeOfGameService.GetTypeOfGameByIdAsync(id);
        //     return response is not null ? Ok(response) : NotFound();
        // }

        // /// <summary>
        // /// Add Type of Game
        // /// </summary>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpPost]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult<TypeOfGameGetByIdResponse>> CreateTypeOfGame(
        //     TypeOfGameCreateRequest request)
        // {
        //     if (!ModelState.IsValid) {
        //         return BadRequest();
        //     }
        //     var response = await _typeOfGameService.CreateTypeOfGameAsync(request);
        //     return CreatedAtRoute(nameof(GetTypeOfGameById), new { id = response.Id }, response);
        // }

        // /// <summary>
        // /// Delete Type Of Game
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpDelete("{id}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult> DeleteGame(string id)
        // {
        //     var response = await _typeOfGameService.DeleteTypeOfGameAsync(id);
        //     return response ? NoContent() : NotFound();
        // }
    }
}