using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class RanksController : BaseController
    {
        // private readonly IRankService _rankService;

        // public RanksController(IRankService rankService)
        // {
        //     _rankService = rankService;
        // }

        // /// <summary>
        // /// Get Rank by Id
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin, Player
        // /// </remarks>
        // [HttpGet("{id}", Name = "GetRankById")]
        // [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        // public async Task<ActionResult<RankGetByIdResponse>> GetRankById(string id)
        // {
        //     var response = await _rankService.GetRankByIdAsync(id);
        //     return response is not null ? Ok(response) : NotFound();
        // }

        // /// <summary>
        // /// Update Rank
        // /// </summary>
        // /// <param name="id"></param>
        // /// <param name="request"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpPut("{id}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult> UpdateRank(string id, RankUpdateRequest request)
        // {
        //     if (!ModelState.IsValid) {
        //         return BadRequest();
        //     }
        //     var response = await _rankService.UpdateRankAsync(id, request);
        //     return response ? NoContent() : NotFound();
        // }

        // /// <summary>
        // /// Delete Rank for Admin
        // /// </summary>
        // /// <param name="id"></param>
        // /// <returns></returns>
        // /// <remarks>
        // /// Roles Access: Admin
        // /// </remarks>
        // [HttpDelete("{id}")]
        // [Authorize(Roles = AuthConstant.RoleAdmin)]
        // public async Task<ActionResult> DeleteRank(string id)
        // {
        //     var response = await _rankService.DeleteRankAsync(id);
        //     return response ? NoContent() : NotFound();
        // }
    }
}