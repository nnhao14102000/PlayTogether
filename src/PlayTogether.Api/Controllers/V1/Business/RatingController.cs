using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class RatingController : BaseController
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }
        
        /// <summary>
        /// Make Rating Feedback 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{orderId}")]
        [Authorize(Roles = AuthConstant.RoleHirer)]
        public async Task<ActionResult> CreateRatingFeedback(string orderId, RatingCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _ratingService.CreateRatingFeedbackAsync(orderId, request);
            return response ? Ok() : NotFound();
        }

        /// <summary>
        /// Get all rating of a Player
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("{playerId}")]
        [Authorize(Roles = AuthConstant.RoleHirer + "," 
                        + AuthConstant.RolePlayer)]
        public async Task<ActionResult<PagedResult<RatingGetResponse>>> GetAllRatings(string playerId, RatingParameters param)
        {
            var response = await _ratingService.GetAllRatingsAsync(playerId, param);
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
    }
}