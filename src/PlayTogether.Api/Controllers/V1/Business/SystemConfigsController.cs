using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.SystemConfig;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    [Route("api/" + ApiConstants.ServiceName + "/v{api-version:apiVersion}/system-configs")]
    public class SystemConfigsController : BaseController
    {
        private readonly ISystemConfigService _systemConfigService;

        public SystemConfigsController(ISystemConfigService systemConfigService)
        {
            _systemConfigService = systemConfigService;
        }

        /// <summary>
        /// Create system config
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> CreateSystemConfig (ConfigCreateRequest request){
            var response = await _systemConfigService.CreateConfigAsync(request);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return Ok(response);
        }
        
        /// <summary>
        /// Get all configs
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllConfig ([FromQuery] SystemConfigParameters param){
            var response = await _systemConfigService.GetAllSystemConfigAsync(param);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return Ok(response);
        }

        /// <summary>
        /// Get config by Id
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User
        /// </remarks>
        [HttpGet, Route("{configId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser)]
        public async Task<ActionResult> GetAllConfigById (string configId){
            var response = await _systemConfigService.GetSystemConfigByIdAsync(configId);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return Ok(response);
        }

        /// <summary>
        /// Delete config
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpDelete, Route("{configId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> DeleteSystemConfig (string configId){
            var response = await _systemConfigService.DeleteConfigAsync(configId);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Update config
        /// </summary>
        /// <param name="configId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin
        /// </remarks>
        [HttpPut, Route("{configId}")]
        [Authorize(Roles = AuthConstant.RoleAdmin)]
        public async Task<ActionResult> UpdateConfigValue (string configId, ConfigUpdateRequest request){
            var response = await _systemConfigService.UpdateConfigAsync(configId, request);
            if(!response.IsSuccess){
                if(response.Error.Code == 404){
                    return NotFound(response);
                }
                else{
                    return BadRequest(response);
                }
            }
            return NoContent();
        }
    }
}