using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Momo;
using PlayTogether.Core.Interfaces.Services.Momo;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class MomoController : BaseController
    {
        private readonly IMomoService _momoService;

        public MomoController(IMomoService momoService)
        {
            _momoService = momoService;
        }

        /// <summary>
        /// Get momo payment link
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> GetLink(WebPaymentRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _momoService.GenerateMomoLinkAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Get IPN Response
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("ipn")]
        public async Task<ActionResult> GetIPNResponse(MomoIPNRequest request){
            return Ok(request);
        }
    }
}