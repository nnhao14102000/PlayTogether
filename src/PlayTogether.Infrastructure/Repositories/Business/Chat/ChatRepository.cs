using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Incoming.Generic;
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

        public async Task<Result<bool>> CreateChatAsync(
            ClaimsPrincipal principal,
            string receiveId,
            ChatCreateRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var model = _mapper.Map<Core.Entities.Chat>(request);
            model.UserId = user.Id;
            model.ReceiveId = receiveId;
            model.UpdateDate = null;

            _context.Chats.Add(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<ChatGetResponse>> GetAllChatsAsync(
            ClaimsPrincipal principal,
            string receiveId,
            ChatParameters param)
        {
            var result = new PagedResult<ChatGetResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;
            
            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }


            var chats = await _context.Chats.Where(x => (x.UserId == user.Id && x.ReceiveId == receiveId) 
                                                        || (x.UserId == receiveId && x.ReceiveId == user.Id))
                                            .OrderByDescending(x => x.CreatedDate)
                                            .ToListAsync();
            foreach (var item in chats)
            {
                await _context.Entry(item).Reference(x => x.User).LoadAsync();
            }
            
            var response = _mapper.Map<List<ChatGetResponse>>(chats);
            foreach (var item in response)
            {
                if(item.IsActive is false) item.Message = "Tin nhắn đã thu hồi";
            }
            return PagedResult<ChatGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        public async Task<Result<bool>> RemoveChatAsync(ClaimsPrincipal principal, string chatId)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var chat = await _context.Chats.FindAsync(chatId);    
            if (chat is null) {                
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + " tin nhắn này.");
                return result;
            }

            if (chat.UserId != user.Id) {
                 result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }
            chat.IsActive = false;
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}