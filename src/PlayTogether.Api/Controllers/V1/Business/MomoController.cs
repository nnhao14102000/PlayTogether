using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Momo;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Interfaces.Services.Momo;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class MomoController : BaseController
    {
        private readonly IMomoService _momoService;
        private readonly ITransactionHistoryService _transactionHistoryService;

        public MomoController(IMomoService momoService, ITransactionHistoryService transactionHistoryService)
        {
            _momoService = momoService;
            _transactionHistoryService = transactionHistoryService;
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
        public async Task<ActionResult> GetIPNResponse([FromBody] MomoIPNRequest request)
        {
            if (request.ResultCode == 0) {
                var response = await _transactionHistoryService.Deposit_v2_Async(request.OrderId.Split("_")[0], (float)request.Amount, request.TransId.ToString());

                if (!response.IsSuccess) {
                    if (response.Error.Code == 404) {
                        return NotFound(response);
                    }
                    else {
                        return BadRequest(response);
                    }
                }
                return Ok(response);
            }
            return NoContent();
        }
    }
}