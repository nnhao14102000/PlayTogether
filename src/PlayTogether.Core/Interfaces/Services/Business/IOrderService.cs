using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IOrderService
    {
        // Task<OrderGetResponse> CreateOrderRequestByHirerAsync(ClaimsPrincipal principal, string playerId, OrderCreateRequest request);
        // Task<OrderGetResponse> GetOrderByIdAsync(string id);
        // Task<PagedResult<OrderGetResponse>> GetAllOrderRequestByPlayerAsync(ClaimsPrincipal principal , PlayerOrderParameter param);
        // Task<PagedResult<OrderGetResponse>> GetAllOrderRequestByHirerAsync(ClaimsPrincipal principal, HirerOrderParameter param);
        // Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(string userId, AdminOrderParameters param);
        // Task<OrderGetDetailResponse> GetOrderByIdInDetailForAdminAsync(string orderId);
        // Task<bool> ProcessOrderRequestByPlayerAsync(string id, ClaimsPrincipal principal, OrderProcessByPlayerRequest request);
        // Task<bool> CancelOrderRequestByHirerAsync(string id, ClaimsPrincipal principal);
        // Task<bool> FinishOrderAsync(string id);
        // Task<bool> FinishOrderSoonAsync(string id, ClaimsPrincipal principal, FinishSoonRequest request);
    }
}