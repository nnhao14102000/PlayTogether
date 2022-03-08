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

            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            string senderId = "";
            if (admin is not null) senderId = admin.Id;
            if (player is not null) senderId = player.Id;
            if (hirer is not null) senderId = hirer.Id;
            if (charity is not null) senderId = charity.Id;

            var model = _mapper.Map<Entities.Chat>(request);
            model.SenderId = senderId;
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

            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            string senderId = "";
            if (admin is not null) senderId = admin.Id;
            if (player is not null) senderId = player.Id;
            if (hirer is not null) senderId = hirer.Id;
            if (charity is not null) senderId = charity.Id;

            var chats = await _context.Chats.Where(x => (x.SenderId == senderId && x.ReceiveId == receiveId) 
                                                        || (x.SenderId == receiveId && x.ReceiveId == senderId))
                                            .OrderBy(x => x.CreatedDate)
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

            var admin = await _context.Admins.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            string senderId = "";
            if (admin is not null) senderId = admin.Id;
            if (player is not null) senderId = player.Id;
            if (hirer is not null) senderId = hirer.Id;
            if (charity is not null) senderId = charity.Id;


            var chat = await _context.Chats.FindAsync(chatId);    
            if (chat is null || chat.SenderId != senderId) {
                return false;
            }
            chat.IsActive = false;
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}