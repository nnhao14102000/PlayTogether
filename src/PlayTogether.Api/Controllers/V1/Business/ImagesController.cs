using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Interfaces.Services.Business;
using System.Threading.Tasks;
using PlayTogether.Api.Helpers;

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
        /// <param name="imageId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("{imageId}", Name = "GetImageById")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> GetImageById(string imageId)
        {
            var response = await _imageService.GetImageByIdAsync(imageId);
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

        /// <summary>
        /// Add New an Image
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateImage(ImageCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _imageService.CreateImageAsync(request);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return CreatedAtRoute(nameof(GetImageById), new { imageId = response.Content.Id }, response);
        }

        /// <summary>
        /// Add multi Images
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost("multi-images")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateMultiImage(IList<ImageCreateRequest> request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _imageService.CreateMultiImageAsync(request);
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

        /// <summary>
        /// Delete an Image
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete("{imageId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteImage(string imageId)
        {
            var response = await _imageService.DeleteImageAsync(imageId);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return NoContent();
        }
        /// <summary>
        /// Delete multi Images
        /// </summary>
        /// <param name="listImageId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> DeleteMultiImage(IList<string> listImageId)
        {
            var response = await _imageService.DeleteMultiImageAsync(listImageId);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }
            return NoContent();
        }
    }
}