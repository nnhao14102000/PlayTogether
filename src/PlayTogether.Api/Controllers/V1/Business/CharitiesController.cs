using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Interfaces.Services.Business.Admin;
using PlayTogether.Core.Interfaces.Services.Business.Charity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class CharitiesController : BaseController
    {
        private readonly ICharityService _charityService;
        public CharitiesController(ICharityService charityService)
        {
            _charityService = charityService;
        }

        /// <summary>
        /// Get all admins
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharityResponse>>> GetAllChairities()
        {
            var response = await _charityService.GetAllCharityAsync().ConfigureAwait(false);
            return response != null ? Ok(response) : NotFound();
        }
    }
}
