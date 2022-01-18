using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
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
        /// Get all Hirers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HirerResponse>>> GetAllHirers()
        {
            var response = await _hirerService.GetAllHirerAsync().ConfigureAwait(false);
            return response != null ? Ok(response) : NotFound();
        }
    }
}
