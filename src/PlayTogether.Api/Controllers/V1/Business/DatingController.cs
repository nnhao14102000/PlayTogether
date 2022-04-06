using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class DatingController : BaseController
    {
        private readonly IDatingService _datingService;

        public DatingController(IDatingService datingService)
        {
            _datingService = datingService;
        }

        /// <summary>
        /// Create Dating
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateDating(DatingCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _datingService.CreateDatingAsync(HttpContext.User, request);
            return response ? Ok() : BadRequest();
        }   

        /// <summary>
        /// Delete Dating
        /// </summary>
        /// <param name="datingId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete("{datingId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteDating(string datingId)
        {
            var response = await _datingService.DeleteDatingAsync(HttpContext.User, datingId);
            return response ? Ok() : BadRequest();
        }
    }
}