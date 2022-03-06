using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;
namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IOrderRepository
    {
        Task<OrderGetResponse> CreateOrderRequestByHirerAsync(ClaimsPrincipal principal, string playerId, OrderCreateRequest request);
        Task<OrderGetResponse> GetOrderByIdAsync (string orderId);
        Task<PagedResult<OrderGetResponse>> GetAllOrderRequestByPlayerAsync(ClaimsPrincipal principal , PlayerOrderParameter param);
        Task<PagedResult<OrderGetResponse>> GetAllOrderRequestByHirerAsync(ClaimsPrincipal principal, HirerOrderParameter param);
        Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(string userId, AdminOrderParameters param);
        Task<OrderGetDetailResponse> GetOrderByIdInDetailForAdminAsync(string orderId);
        Task<bool> ProcessOrderRequestByPlayerAsync(string orderId, ClaimsPrincipal principal, OrderProcessByPlayerRequest request);
        Task<bool> CancelOrderRequestByHirerAsync(string orderId, ClaimsPrincipal principal);
        Task<bool> FinishOrderAsync(string orderId);
        Task<bool> FinishOrderSoonAsync(string orderId, ClaimsPrincipal principal, FinishSoonRequest request);
        // Task<OrderGetByIdForPlayer> GetOrderByIdForPlayerAsync(string id);
    }
}