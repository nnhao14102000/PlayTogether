using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Order
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public OrderRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<OrderGetByIdResponse> CreateOrderRequestByHirerAsync(
            ClaimsPrincipal principal,
            string playerId,
            OrderCreateRequest request)
        {
            // Check Hirer
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (hirer is null || hirer.IsActive is false) {
                return null;
            }

            // Check player and status of player
            var player = await _context.Players.FindAsync(playerId);
            if (player is null
                || player.Status is not PlayerStatusConstants.Ready
                || player.IsActive is false) {
                return null;
            }

            // Check balance of hirer
            if ((request.TotalTimes * player.PricePerHour) < hirer.Balance) {
                return null;
            }

            var model = _mapper.Map<Entities.Order>(request);
            model.PlayerId = playerId;
            model.HirerId = hirer.Id;
            model.TotalPrices = request.TotalTimes * player.PricePerHour;
            model.Status = OrderStatusConstant.Processing; // change status of Order

            _context.Orders.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                player.Status = PlayerStatusConstants.Processing; // change status of Player
                if ((await _context.SaveChangesAsync() < 0)) {
                    return null;
                }
                return _mapper.Map<OrderGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByHirerAsync(
            ClaimsPrincipal principal,
            HirerOrderParameter param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (hirer is null) {
                return null;
            }

            var ordersOfHirer = await _context.Orders.Where(x => x.HirerId == hirer.Id).ToListAsync();
            var response = _mapper.Map<List<OrderGetByIdResponse>>(ordersOfHirer);
            return PagedResult<OrderGetByIdResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        public async Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByPlayerAsync(ClaimsPrincipal principal, PlayerOrderParameter param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (player is null) {
                return null;
            }

            var ordersOfPlayer = await _context.Orders.Where(x => x.PlayerId == player.Id).ToListAsync();
            var response = _mapper.Map<List<OrderGetByIdResponse>>(ordersOfPlayer);
            return PagedResult<OrderGetByIdResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        public async Task<OrderGetByIdResponse> GetOrderByIdAsync(string id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null) {
                return null;
            }
            return _mapper.Map<OrderGetByIdResponse>(order);
        }

        public async Task<bool> CancelOrderRequestByHirerAsync(string id, ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (hirer is null) {
                return false;
            }

            var order = await _context.Orders.FindAsync(id);
            if (order is null) {
                return false;
            }

            if (order.Status is OrderStatusConstant.Cancel
                || order.Status is OrderStatusConstant.Interrupt
                || order.Status is OrderStatusConstant.Start
                || order.Status is OrderStatusConstant.Finish ) {
                return false;
            }

            order.Status = OrderStatusConstant.Cancel; // change status of Order

            await _context.Entry(order)
               .Reference(x => x.Player)
               .LoadAsync();
               
            order.Player.Status = PlayerStatusConstants.Ready; // Cancel Order => Change status of Player to Ready

            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<bool> ProcessOrderRequestByPlayerAsync(string id, ClaimsPrincipal principal, OrderProcessByPlayerRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var player = await _context.Players.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (player is null) {
                return false;
            }

            var order = await _context.Orders.FindAsync(id);
            if (order is null) {
                return false;
            }

            if (order.Status is OrderStatusConstant.Cancel || order.Status is OrderStatusConstant.Interrupt) {
                return false;
            }

            if (request.IsAccept == false) {
                player.Status = PlayerStatusConstants.Ready; // Reject Order => Change status of Player to Ready
                order.Status = OrderStatusConstant.Cancel;
            }
            else {
                await _context.Entry(order).Reference(x => x.Hirer).LoadAsync();
                player.Status = PlayerStatusConstants.Hired; // If Accept => Change status of Player to Hired
                order.TimeStart = DateTime.Now; // Time start
                // Change balance
                player.Balance = player.Balance + order.TotalPrices;
                order.Hirer.Balance = order.Hirer.Balance - order.TotalPrices;
                order.Status = OrderStatusConstant.Start; // change status of Order
            }

            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;

        }

        public async Task<bool> FinishOrderAsync(string id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is null) {
                return false;
            }

            if (order.Status is OrderStatusConstant.Cancel
                || order.Status is OrderStatusConstant.Interrupt
                || order.Status is OrderStatusConstant.Processing
                || order.Status is OrderStatusConstant.Finish) {
                return false;
            }

            await _context.Entry(order).Reference(x => x.Player).LoadAsync();
            order.Status = OrderStatusConstant.Finish;
            order.Player.Status = PlayerStatusConstants.Ready;

            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }
    }
}