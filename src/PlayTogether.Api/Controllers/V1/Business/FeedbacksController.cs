using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class FeedbacksController : BaseController
    {
        private readonly ISystemFeedbackService _systemFeedbackService;

        public FeedbacksController(ISystemFeedbackService systemFeedbackService)
        {
            _systemFeedbackService = systemFeedbackService;
        }

        /// <summary>
        /// Create feedback
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateFeedback(CreateFeedbackRequest request){
            if(!ModelState.IsValid){
                return BadRequest();
            }
            var response = await _systemFeedbackService.CreateFeedbackAsync(HttpContext.User, request);
            return response ? Ok() : NotFound();
        }

        /// <summary>
        /// Delete feedback
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete, Route("{feedbackId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteFeedback(string feedbackId){
            var response = await _systemFeedbackService.DeleteFeedbackAsync(HttpContext.User, feedbackId);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Update feedback
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPut, Route("{feedbackId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> UpdateFeedback(string feedbackId, UpdateFeedbackRequest request){
            if(!ModelState.IsValid){
                return BadRequest();
            }
            var response = await _systemFeedbackService.UpdateFeedbackAsync(HttpContext.User, feedbackId, request);
            return response ? NoContent() : NotFound();
        }       

        /// <summary>
        /// Get feedback by Id
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User, Admin
        /// </remarks>
        [HttpGet, Route("{feedbackId}")]
        [Authorize(Roles = AuthConstant.RoleUser + "," + AuthConstant.RoleAdmin)]
        public async Task<ActionResult<SystemFeedbackDetailResponse>> GetFeedbackById(string feedbackId){
            var response = await _systemFeedbackService.GetFeedbackByIdAsync(feedbackId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get all feedbacks
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User, Admin
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleUser + "," + AuthConstant.RoleAdmin)]
        public async Task<ActionResult<PagedResult<SystemFeedbackResponse>>> GetAllFeedbacks(
            [FromQuery] SystemFeedbackParameters param){
            var response = await _systemFeedbackService.GetAllFeedbacksAsync(HttpContext.User, param);
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