using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Chat
{
    public class ChatRepository : BaseRepository, IChatRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ChatRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateChatAsync(
            ClaimsPrincipal principal,
            string receiveId,
            ChatCreateRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var model = _mapper.Map<Entities.Chat>(request);
            model.UserId = user.Id;
            model.ReceiveId = receiveId;
            model.UpdateDate = null;

            _context.Chats.Add(model);

            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<ChatGetResponse>> GetAllChatsAsync(
            ClaimsPrincipal principal,
            string receiveId,
            ChatParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id;

            
            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var chats = await _context.Chats.Where(x => (x.UserId == user.Id && x.ReceiveId == receiveId) 
                                                        || (x.UserId == receiveId && x.ReceiveId == user.Id))
                                            .OrderByDescending(x => x.CreatedDate)
                                            .ToListAsync();
            
            var response = _mapper.Map<List<ChatGetResponse>>(chats);
            foreach (var item in response)
            {
                if(item.IsActive is false) item.Message = "Tin nhắn đã thu hồi";
            }
            return PagedResult<ChatGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<bool> RemoveChatAsync(ClaimsPrincipal principal, string chatId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);


            var chat = await _context.Chats.FindAsync(chatId);    
            if (chat is null || chat.UserId != user.Id) {
                return false;
            }
            chat.IsActive = false;
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}