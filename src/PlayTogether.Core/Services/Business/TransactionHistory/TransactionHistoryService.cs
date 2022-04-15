using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.TransactionHistory
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;
        private readonly ILogger<TransactionHistoryService> _logger;

        public TransactionHistoryService(ITransactionHistoryRepository transactionHistoryRepository, ILogger<TransactionHistoryService> logger)
        {
            _transactionHistoryRepository = transactionHistoryRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> DepositAsync(ClaimsPrincipal principal, DepositRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _transactionHistoryRepository.DepositAsync(principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call DepositAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<TransactionHistoryResponse>> GetAllTransactionHistoriesAsync(ClaimsPrincipal principal, TransactionParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _transactionHistoryRepository.GetAllTransactionHistoriesAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllTransactionHistoriesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<TransactionHistoryResponse>> GetAllTransactionHistoriesForAdminAsync(string userId, TransactionParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrWhiteSpace(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _transactionHistoryRepository.GetAllTransactionHistoriesForAdminAsync(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllTransactionHistoriesForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}