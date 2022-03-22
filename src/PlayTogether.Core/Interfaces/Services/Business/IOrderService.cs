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
        Task<OrderGetResponse> CreateOrderAsync(ClaimsPrincipal principal, string toUserId, OrderCreateRequest request);
        Task<OrderGetResponse> GetOrderByIdAsync (ClaimsPrincipal principal, string orderId);
        Task<PagedResult<OrderGetResponse>> GetAllOrdersAsync(ClaimsPrincipal principal , UserOrderParameter param);
        Task<PagedResult<OrderGetResponse>> GetAllOrderRequestsAsync(ClaimsPrincipal principal, UserOrderParameter param);
        Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(string userId, AdminOrderParameters param);
        Task<OrderGetDetailResponse> GetOrderByIdInDetailForAdminAsync(string orderId);
        Task<bool> ProcessOrderAsync(string orderId, ClaimsPrincipal principal, OrderProcessByPlayerRequest request);
        Task<bool> CancelOrderAsync(string orderId, ClaimsPrincipal principal);
        Task<bool> FinishOrderAsync(string orderId);
        Task<bool> FinishOrderSoonAsync(string orderId, ClaimsPrincipal principal, FinishSoonRequest request);
    }
}