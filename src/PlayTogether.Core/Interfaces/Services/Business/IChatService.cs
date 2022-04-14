using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IChatService
    {
        Task<Result<bool>> CreateChatAsync(ClaimsPrincipal principal, string receiveId, ChatCreateRequest request);
        Task<PagedResult<ChatGetResponse>> GetAllChatsAsync(ClaimsPrincipal principal, string receiveId, ChatParameters param);
        Task<Result<bool>> RemoveChatAsync(ClaimsPrincipal principal, string chatId);
    }
}