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
        Task<Result<OrderGetResponse>> CreateOrderAsync(ClaimsPrincipal principal, string toUserId, OrderCreateRequest request);
        Task<Result<OrderGetResponse>> GetOrderByIdAsync (ClaimsPrincipal principal, string orderId);
        Task<PagedResult<OrderGetResponse>> GetAllOrdersAsync(ClaimsPrincipal principal , UserOrderParameter param);
        Task<PagedResult<OrderGetResponse>> GetAllOrderRequestsAsync(ClaimsPrincipal principal, UserOrderParameter param);
        Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(string userId, AdminOrderParameters param);
        Task<Result<OrderGetDetailResponse>> GetOrderByIdInDetailAsync(string orderId);
        Task<Result<bool>> ProcessOrderAsync(string orderId, ClaimsPrincipal principal, OrderProcessByPlayerRequest request);
        Task<Result<bool>> CancelOrderAsync(string orderId, ClaimsPrincipal principal, OrderCancelRequest request);
        Task<Result<bool>> FinishOrderAsync(string orderId);
        Task<Result<bool>> FinishOrderSoonAsync(string orderId, ClaimsPrincipal principal, FinishSoonRequest request);
    }
}