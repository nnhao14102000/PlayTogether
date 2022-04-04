using System;
using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.SystemFeedback
{
    public class SystemFeedbackService : ISystemFeedbackService
    {
        private readonly ISystemFeedbackRepository _systemFeedbackRepository;
        private readonly ILogger<SystemFeedbackService> _logger;

        public SystemFeedbackService(ISystemFeedbackRepository systemFeedbackRepository, ILogger<SystemFeedbackService> logger)
        {
            _systemFeedbackRepository = systemFeedbackRepository;
            _logger = logger;
        }
        public async Task<bool> CreateFeedbackAsync(ClaimsPrincipal principal, CreateFeedbackRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _systemFeedbackRepository.CreateFeedbackAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateFeedbackAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> DeleteFeedbackAsync(ClaimsPrincipal principal, string feedbackId)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(feedbackId) || String.IsNullOrWhiteSpace(feedbackId)) {
                    throw new ArgumentNullException(nameof(feedbackId));
                }
                return await _systemFeedbackRepository.DeleteFeedbackAsync(principal, feedbackId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DeleteFeedbackAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<SystemFeedbackResponse>> GetAllFeedbacksAsync(ClaimsPrincipal principal, SystemFeedbackParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _systemFeedbackRepository.GetAllFeedbacksAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllFeedbacksAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<SystemFeedbackDetailResponse> GetFeedbackByIdAsync(string feedbackId)
        {
            try {
                if (String.IsNullOrEmpty(feedbackId) || String.IsNullOrWhiteSpace(feedbackId)) {
                    throw new ArgumentNullException(nameof(feedbackId));
                }
                return await _systemFeedbackRepository.GetFeedbackByIdAsync(feedbackId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetFeedbackByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> ProcessFeedbackAsync(string feedbackId, ProcessFeedbackRequest request)
        {
            try {
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                if (String.IsNullOrEmpty(feedbackId) || String.IsNullOrWhiteSpace(feedbackId)) {
                    throw new ArgumentNullException(nameof(feedbackId));
                }
                return await _systemFeedbackRepository.ProcessFeedbackAsync(feedbackId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ProcessFeedbackAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> UpdateFeedbackAsync(ClaimsPrincipal principal, string feedbackId, UpdateFeedbackRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                if (String.IsNullOrEmpty(feedbackId) || String.IsNullOrWhiteSpace(feedbackId)) {
                    throw new ArgumentNullException(nameof(feedbackId));
                }
                return await _systemFeedbackRepository.UpdateFeedbackAsync(principal, feedbackId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call UpdateFeedbackAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}