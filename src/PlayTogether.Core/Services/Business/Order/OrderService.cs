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

        public async Task<OrderGetByIdResponse> CreateOrderRequestByHirerAsync(
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

        public async Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByHirerAsync(ClaimsPrincipal principal, HirerOrderParameter param)
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

        public async Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByPlayerAsync(ClaimsPrincipal principal, PlayerOrderParameter param)
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

        public async Task<OrderGetByIdResponse> GetOrderByIdAsync(string id)
        { 
            try {
                if (String.IsNullOrEmpty(id)) {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _orderRepository.GetOrderByIdAsync(id);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetOrderByIdAsync in service class, Error Message: {ex}.");
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