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
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost("{orderId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
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
        /// <param name="userId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{userId}")]
        [Authorize(Roles = AuthConstant.RoleUser + ","
                        + AuthConstant.RoleAdmin)]
        public async Task<ActionResult<PagedResult<RatingGetResponse>>> GetAllRatings(string userId, [FromQuery] RatingParameters param)
        {
            var response = await _ratingService.GetAllRatingsAsync(userId, param);
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
        /// Report violate feedback
        /// </summary>
        /// <param name="rateId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut("violate/{rateId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> ReportViolateFeedback(string rateId)
        {
            var response = await _ratingService.ViolateRatingAsync(rateId);
            return response ? Ok() : NotFound();
        }

        /// <summary>
        /// Get all violate ratings
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpGet("violates")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<PagedResult<RatingGetResponse>>> GetAllViolateRatings([FromQuery] RatingParametersAdmin param)
        {
            var response = await _ratingService.GetAllViolateRatingsForAdminAsync(param);
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
        /// Process violate feedback
        /// </summary>
        /// <param name="rateId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut("process/{rateId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DisableFeedback(string rateId, ProcessViolateRatingRequest request)
        {
            var response = await _ratingService.ProcessViolateRatingAsync(rateId, request);
            return response ? Ok() : NotFound();
        }
    }
}