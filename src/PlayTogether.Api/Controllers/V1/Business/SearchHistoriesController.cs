using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Business.SearchHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/search-histories")]
    public class SearchHistoriesController : BaseController
    {
        private readonly ISearchHistoryService _searchHistoryService;
        public SearchHistoriesController(ISearchHistoryService searchHistoryService)
        {
            _searchHistoryService = searchHistoryService;
        }
        
        /// <summary>
        /// Get all search histories of authentication user
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<SearchHistoryResponse>>> GetAllSearchHistories(
            [FromQuery] SearchHistoryParameters param)
        {
            var response = await _searchHistoryService.GetAllSearchHistoryAsync(HttpContext.User, param);

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
        /// Delete a search history
        /// </summary>
        /// <param name="searchHistoryId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete, Route("{searchHistoryId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteSearchHistory(string searchHistoryId){
            var response = await _searchHistoryService.DeleteSearchHistoryAsync(HttpContext.User, searchHistoryId);
            return response ? NoContent() : BadRequest();
        }
    }
}