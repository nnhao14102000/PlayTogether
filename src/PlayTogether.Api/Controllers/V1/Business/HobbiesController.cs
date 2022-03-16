using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Hobby;
using PlayTogether.Core.Interfaces.Services.Business;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class HobbiesController : BaseController
    {
        private readonly IHobbyService _hobbyService;
        public HobbiesController(IHobbyService hobbyService)
        {
            _hobbyService = hobbyService;
        }

        /// <summary>
        /// Create hobbies
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateHobbies(List<HobbyCreateRequest> requests)
        {
            var response = await _hobbyService.CreateHobbiesAsync(HttpContext.User, requests);
            return response ? Ok() : NotFound();
        }

        /// <summary>
        /// Delete a hobby
        /// </summary>
        /// <param name="hobbyId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete, Route("{hobbyId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteHobby(string hobbyId)
        {
            var response = await _hobbyService.DeleteHobbyAsync(HttpContext.User, hobbyId);
            return response ? NoContent() : NotFound();
        }
    }
}