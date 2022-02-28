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
        [HttpGet("{id}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RolePlayer)]
        public async Task<ActionResult<CharityResponse>> GetCharityById(string id){
            var response = await _charityService.GetCharityByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }


        [HttpGet("number-of-donate")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult> GetNumberOfDonateInDay([FromQuery] DonateParameters param){
            var response = await _donateService.CalculateDonateAsync(HttpContext.User, param);
            return response >= 0 ? Ok(response) : BadRequest();
        }
    }
}
