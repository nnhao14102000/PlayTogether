using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IChatRepository
    {
        Task<bool> CreateChatAsync(ClaimsPrincipal principal, string receiveId, ChatCreateRequest request);
        Task<PagedResult<ChatGetResponse>> GetAllChatsAsync(ClaimsPrincipal principal, string receiveId, ChatParameters param);
        Task<bool> RemoveChatAsync(ClaimsPrincipal principal, string chatId);
    }
}