using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class CharitiesController : BaseController
    {
        private readonly ICharityService _charityService;
        private readonly IDonateService _donateService;

        public CharitiesController(ICharityService charityService, IDonateService donateService)
        {
            _charityService = charityService;
            _donateService = donateService;
        }

        /// <summary>
        /// Get all Charities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, Player
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<PagedResult<CharityResponse>>> GetAllCharities(
            [FromQuery] CharityParameters param)
        {
            var response = await _charityService.GetAllCharitiesAsync(param).ConfigureAwait(false);

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
        /// Get charity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, Player
        /// </remarks>
        [HttpGet("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<CharityResponse>> GetCharityById(string id)
        {
            var response = await _charityService.GetCharityByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Calculate Donate
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity
        /// </remarks>
        [HttpGet("calculate-donate")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult<(int, float, int, float)>> GetNumberOfDonateInDay()
        {
            var response = await _donateService.CalculateDonateAsync(HttpContext.User);
            return response.Item1 >= 0
                   && response.Item2 >= 0
                   && response.Item3 >= 0
                   && response.Item4 >= 0 ? Ok(response) : BadRequest();
        }
    }
}
