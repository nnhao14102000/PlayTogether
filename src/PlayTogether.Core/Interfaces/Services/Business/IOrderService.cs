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
        Task<OrderGetByIdResponse> CreateOrderRequestByHirerAsync(ClaimsPrincipal principal, string playerId, OrderCreateRequest request);
        Task<OrderGetByIdResponse> GetOrderByIdAsync(string id);
        Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByPlayerAsync(ClaimsPrincipal principal , PlayerOrderParameter param);
        Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByHirerAsync(ClaimsPrincipal principal, HirerOrderParameter param);
        Task<bool> ProcessOrderRequestByPlayerAsync(string id, ClaimsPrincipal principal, OrderProcessByPlayerRequest request);
        Task<bool> CancelOrderRequestByHirerAsync(string id, ClaimsPrincipal principal);
        Task<bool> FinishOrderAsync(string id);
        Task<bool> FinishOrderSoonAsync(string id, ClaimsPrincipal principal, FinishSoonRequest request);
    }
}