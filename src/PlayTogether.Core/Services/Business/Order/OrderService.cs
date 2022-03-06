using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Order
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<bool> CancelOrderRequestByHirerAsync(string id, ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _orderRepository.CancelOrderRequestByHirerAsync(id, principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CancelOrderRequestByHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<OrderGetResponse> CreateOrderRequestByHirerAsync(
            ClaimsPrincipal principal,
            string playerId,
            OrderCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(playerId)) {
                    throw new ArgumentNullException(nameof(playerId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _orderRepository.CreateOrderRequestByHirerAsync(principal, playerId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateOrderRequestByHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> FinishOrderAsync(string id)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _orderRepository.FinishOrderAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call FinishOrderAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> FinishOrderSoonAsync(string id, ClaimsPrincipal principal, FinishSoonRequest request)
        {
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _orderRepository.FinishOrderSoonAsync(id, principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call FinishOrderAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(string userId, AdminOrderParameters param)
        {
            try {
                if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(userId)) {
                    throw new ArgumentNullException(nameof(userId));
                }
                return await _orderRepository.GetAllOrderByUserIdForAdminAsync(userId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllOrderByUserIdForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderRequestByHirerAsync(ClaimsPrincipal principal, HirerOrderParameter param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _orderRepository.GetAllOrderRequestByHirerAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllOrderRequestByHirerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderRequestByPlayerAsync(ClaimsPrincipal principal, PlayerOrderParameter param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _orderRepository.GetAllOrderRequestByPlayerAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllOrderRequestByPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<OrderGetResponse> GetOrderByIdAsync(string id)
        { 
            try {
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _orderRepository.GetOrderByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetOrderByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<OrderGetDetailResponse> GetOrderByIdInDetailForAdminAsync(string orderId)
        {
            try {
                if (String.IsNullOrEmpty(orderId) || String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                return await _orderRepository.GetOrderByIdInDetailForAdminAsync(orderId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetOrderByIdInDetailForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<bool> ProcessOrderRequestByPlayerAsync(string id, ClaimsPrincipal principal, OrderProcessByPlayerRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _orderRepository.ProcessOrderRequestByPlayerAsync(id, principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ProcessOrderRequestByPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}