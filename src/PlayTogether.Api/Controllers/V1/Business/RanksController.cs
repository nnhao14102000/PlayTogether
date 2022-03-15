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
        private readonly IRankService _rankService;

        public RanksController(IRankService rankService)
        {
            _rankService = rankService;
        }

        /// <summary>
        /// Get Rank by Id
        /// </summary>
        /// <param name="rankId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{rankId}", Name = "GetRankById")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult<RankGetByIdResponse>> GetRankById(string rankId)
        {
            var response = await _rankService.GetRankByIdAsync(rankId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update Rank
        /// </summary>
        /// <param name="rankId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut("{rankId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateRank(string rankId, RankUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _rankService.UpdateRankAsync(rankId, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete Rank for Admin
        /// </summary>
        /// <param name="rankId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete("{rankId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteRank(string rankId)
        {
            var response = await _rankService.DeleteRankAsync(rankId);
            return response ? NoContent() : NotFound();
        }
    }
}