using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class ImagesController : BaseController
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Get Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Player, Hirer
        /// </remarks>
        [HttpGet("{id}", Name = "GetImageById")]
        [Authorize(Roles = AuthConstant.RolePlayer + "," + AuthConstant.RoleHirer)]
        public async Task<ActionResult<ImageGetByIdResponse>> GetImageById(string id)
        {
            var response = await _imageService.GetImageByIdAsync(id);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Add New an Image
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Player
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult<ImageGetByIdResponse>> CreateImage(ImageCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _imageService.CreateImageAsync(request);
            return response is null ? BadRequest() : CreatedAtRoute(nameof(GetImageById), new { id = response.Id }, response);
        }

        /// <summary>
        /// Delete an Image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Player
        /// </remarks>
        [HttpDelete("{id}")]
        [Authorize(Roles = AuthConstant.RolePlayer)]
        public async Task<ActionResult> DeleteImage(string id)
        {
            var response = await _imageService.DeleteImageAsync(id);
            return response ? NoContent() : NotFound();
        }
    }
}