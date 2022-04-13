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

        public async Task<Result<bool>> CancelOrderAsync(string orderId, ClaimsPrincipal principal)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                return await _orderRepository.CancelOrderAsync(orderId, principal);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CancelOrderAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<OrderGetResponse>> CreateOrderAsync(
            ClaimsPrincipal principal,
            string toUserId,
            OrderCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(toUserId)) {
                    throw new ArgumentNullException(nameof(toUserId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _orderRepository.CreateOrderAsync(principal, toUserId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateOrderAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> FinishOrderAsync(string orderId)
        {
            try {
                if (String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                return await _orderRepository.FinishOrderAsync(orderId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call FinishOrderAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> FinishOrderSoonAsync(string orderId, ClaimsPrincipal principal, FinishSoonRequest request)
        {
            try {
                if (String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _orderRepository.FinishOrderSoonAsync(orderId, principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call FinishOrderSoonAsync in service class, Error Message: {ex}.");
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

        public async Task<PagedResult<OrderGetResponse>> GetAllOrdersAsync(ClaimsPrincipal principal, UserOrderParameter param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _orderRepository.GetAllOrdersAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllOrdersAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderRequestsAsync(ClaimsPrincipal principal, UserOrderParameter param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                return await _orderRepository.GetAllOrderRequestsAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllOrderRequestsAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<OrderGetResponse>> GetOrderByIdAsync(ClaimsPrincipal principal, string id)
        { 
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(id) || String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _orderRepository.GetOrderByIdAsync(principal, id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetOrderByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<OrderGetDetailResponse>> GetOrderByIdInDetailAsync(string orderId)
        {
            try {
                if (String.IsNullOrEmpty(orderId) || String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                return await _orderRepository.GetOrderByIdInDetailAsync(orderId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetOrderByIdInDetailAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> ProcessOrderAsync(string id, ClaimsPrincipal principal, OrderProcessByPlayerRequest request)
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
                return await _orderRepository.ProcessOrderAsync(id, principal, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call ProcessOrderAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}