using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;
using System.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using Microsoft.Extensions.Configuration;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class CharitiesController : BaseController
    {
        private readonly ICharityService _charityService;
        private readonly IDonateService _donateService;
        private readonly string _azureConnectionString;

        public CharitiesController(ICharityService charityService, IDonateService donateService, IConfiguration configuration)
        {
            _charityService = charityService;
            _donateService = donateService;
            _azureConnectionString = configuration.GetConnectionString("ImageAzureConnectionString");
        }

        /// <summary>
        /// Get all Charities
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
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
        /// <param name="charityId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet("{charityId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult<CharityResponse>> GetCharityById(string charityId)
        {
            var response = await _charityService.GetCharityByIdAsync(charityId);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Get charity profile 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity
        /// </remarks>
        [HttpGet("personal")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult<CharityResponse>> GetCharityProfile()
        {
            var response = await _charityService.GetProfileAsync(HttpContext.User);
            return response is not null ? Ok(response) : NotFound();
        }

        /// <summary>
        /// Update charity profile 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity, Admin
        /// </remarks>
        [HttpPut("{charityId}")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult> UpdateCharityProfile(string charityId, CharityUpdateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _charityService.UpdateProfileAsync(HttpContext.User, charityId, request);
            return response ? NoContent() : NotFound();
        }

        /// <summary>
        /// Calculate Donate
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity
        /// </remarks>
        [HttpGet("dash-board")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult<(int, float, int, float)>> GetNumberOfDonateInDay()
        {
            var response = await _donateService.CalculateDonateAsync(HttpContext.User);
            return response.Item1 >= 0
                   && response.Item2 >= 0
                   && response.Item3 >= 0
                   && response.Item4 >= 0 ? Ok(response) : BadRequest();
        }

        /// <summary>
        /// Update image
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Role Access: Charity
        /// </remarks>
        [HttpPost("images")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
		public async Task<IActionResult> Upload()
		{
			try
			{
				var formCollection = await Request.ReadFormAsync();
				var file = formCollection.Files.First();

				if (file.Length > 0)
				{
					var container = new BlobContainerClient(_azureConnectionString, "upload-container");
					var createResponse = await container.CreateIfNotExistsAsync();
					if (createResponse != null && createResponse.GetRawResponse().Status == 201)
						await container.SetAccessPolicyAsync(PublicAccessType.Blob);

					var blob = container.GetBlobClient(file.FileName);
					await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

					using (var fileStream = file.OpenReadStream())
					{
						await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
					}

					return Ok(blob.Uri.ToString());
				}

				return BadRequest();
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex}");
			}
		}
    }
}
