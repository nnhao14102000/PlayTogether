using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Chat
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _logger = logger;
        }
        public async Task<bool> CreateChatAsync(
            ClaimsPrincipal principal,
            string receiveId,
            ChatCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                if (String.IsNullOrEmpty(receiveId)) {
                    throw new ArgumentNullException(nameof(receiveId));
                }
                return await _chatRepository.CreateChatAsync(principal, receiveId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateChatAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<ChatGetResponse>> GetAllChatsAsync(
            ClaimsPrincipal principal,
            string receiveId,
            ChatParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(receiveId)) {
                    throw new ArgumentNullException(nameof(receiveId));
                }
                return await _chatRepository.GetAllChatsAsync(principal, receiveId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllChatsAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> RemoveChatAsync(ClaimsPrincipal principal, string chatId)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(chatId)) {
                    throw new ArgumentNullException(nameof(chatId));
                }
                return await _chatRepository.RemoveChatAsync(principal, chatId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call RemoveChatAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}