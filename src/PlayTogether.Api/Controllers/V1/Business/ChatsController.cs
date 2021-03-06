using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class ChatsController : BaseController
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// Create a chat message
        /// </summary>
        /// <param name="receiveId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpPost("{receiveId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> CreateChat(string receiveId, ChatCreateRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest();
            }
            var response = await _chatService.CreateChatAsync(HttpContext.User, receiveId, request);
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
        /// Get all chats
        /// </summary>
        /// <param name="receiveId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpGet("{receiveId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult<PagedResult<ChatGetResponse>>> GetAllChats(
            string receiveId,
            [FromQuery] ChatParameters param)
        {
            var response = await _chatService.GetAllChatsAsync(HttpContext.User, receiveId, param);
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
        /// Remove chat message
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: User
        /// </remarks>
        [HttpDelete("{chatId}")]
        [Authorize(Roles = AuthConstant.RoleUser)]
        public async Task<ActionResult> RemoveChat(string chatId)
        {
            var response = await _chatService.RemoveChatAsync(HttpContext.User, chatId);
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