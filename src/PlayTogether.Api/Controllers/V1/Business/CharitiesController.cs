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
        private readonly ICharityWithdrawService _charityWithdrawService;

        public CharitiesController(ICharityService charityService, IDonateService donateService, IConfiguration configuration, ICharityWithdrawService charityWithdrawService)
        {
            _charityService = charityService;
            _donateService = donateService;
            _charityWithdrawService = charityWithdrawService;
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
        public async Task<ActionResult> GetAllCharities(
            [FromQuery] CharityParameters param)
        {
            var response = await _charityService.GetAllCharitiesAsync(param).ConfigureAwait(false);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }

            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        /// <summary>
        /// Get all withdraw history of a specific charity
        /// </summary>
        /// <param name="charityId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet, Route("{charityId}/withdraw-histories")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleCharity)]
        public async Task<ActionResult> GetAllCharityWithdrawHistories(string charityId, CharityWithdrawParameters param)
        {
            var response = await _charityWithdrawService.GetAllCharityWithdrawHistoriesAsync(charityId, param);
            if (!response.IsSuccess) {
                if (response.Error.Code == 404) {
                    return NotFound(response);
                }
                else {
                    return BadRequest(response);
                }
            }

            var metaData = new {
                response.TotalCount,
                response.PageSize,
                response.CurrentPage,
                response.HasNext,
                response.HasPrevious
            };

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
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
        public async Task<ActionResult> GetCharityById(string charityId)
        {
            var response = await _charityService.GetCharityByIdAsync(charityId);
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
        /// Get charity profile 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity
        /// </remarks>
        [HttpGet("personal")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult> GetCharityProfile()
        {
            var response = await _charityService.GetProfileAsync(HttpContext.User);
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
        /// Calculate Donate
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity
        /// </remarks>
        [HttpGet("dash-board")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult> GetNumberOfDonateInDay()
        {
            var response = await _donateService.CalculateDonateAsync(HttpContext.User);
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
        /// With draw money
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Charity
        /// </remarks>
        [HttpPut("with-draw")]
        [Authorize(Roles = AuthConstant.RoleCharity)]
        public async Task<ActionResult> WithdrawMoney(CharityWithDrawRequest request)
        {
            var response = await _charityService.CharityWithDrawAsync(HttpContext.User, request);
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
            try {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();

                if (file.Length > 0) {
                    var container = new BlobContainerClient(_azureConnectionString, "upload-container");
                    var createResponse = await container.CreateIfNotExistsAsync();
                    if (createResponse != null && createResponse.GetRawResponse().Status == 201)
                        await container.SetAccessPolicyAsync(PublicAccessType.Blob);

                    var blob = container.GetBlobClient(file.FileName);
                    await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

                    using (var fileStream = file.OpenReadStream()) {
                        await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = file.ContentType });
                    }

                    return Ok(blob.Uri.ToString());
                }

                return BadRequest();
            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
