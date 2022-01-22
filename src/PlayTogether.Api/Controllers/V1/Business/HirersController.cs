using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
using PlayTogether.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class HirersController: BaseController
    {
        private readonly IHirerService _hirerService;
        
        public HirersController(IHirerService HirerService)
        {
            _hirerService = HirerService;
        }

        /// <summary>
        /// Get all hirers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult<IEnumerable<HirerResponse>>> GetAllHirers(
            [FromQuery] HirerParameters param)
        {
            var response = await _hirerService.GetAllHirersAsync(param).ConfigureAwait(false);

            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));
            
            return response != null ? Ok(response) : NotFound();
        }
    }
}
