using System.Net;
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

        public async Task<OrderGetResponse> CreateOrderAsync(
            ClaimsPrincipal principal,
            string toUserId,
            OrderCreateRequest request)
        {
            // Check Hirer
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null
                || user.IsActive is false
                || user.IsPlayer is true
                || user.Status is not UserStatusConstants.Online) {
                return null;
            }

            // Check player and status of player
            var toUser = await _context.AppUsers.FindAsync(toUserId);
            if (toUser is null
                || toUser.Status is not UserStatusConstants.Online
                || toUser.IsActive is false
                || toUser.IsPlayer is false) {
                return null;
            }

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();
            await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();

            if (request.TotalTimes > toUser.MaxHourHire || request.TotalTimes < 1) {
                return null;
            }

            // Check balance of hirer
            if ((request.TotalTimes * toUser.PricePerHour) > user.UserBalance.ActiveBalance) {
                return null;
            }

            if (request.Games.Count == 0) {
                return null;
            }

            IEnumerable<GamesOfOrderCreateRequest> duplicates = request.Games.GroupBy(x => x)
                                        .SelectMany(g => g.Skip(1));
            if (duplicates.Count() > 0) {
                return null;
            }

            foreach (var game in request.Games) {
                var isSkill = await _context.GameOfUsers.Where(x => x.UserId == toUserId)
                                                            .AnyAsync(x => x.GameId == game.GameId);
                if (!isSkill) return null;
            }

            var model = _mapper.Map<Entities.Order>(request);
            model.ToUserId = toUserId;
            model.UserId = user.Id;
            model.TotalPrices = request.TotalTimes * toUser.PricePerHour;
            model.Status = OrderStatusConstants.Processing;
            model.ProcessExpired = DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.OrderProcessExpireTime);

            _context.Orders.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                foreach (var game in request.Games) {
                    var existGame = await _context.GameOfOrders.Where(x => x.OrderId == model.Id)
                                                                .AnyAsync(x => x.GameId == game.GameId);
                    if (existGame) continue;
                    var g = _mapper.Map<Entities.GameOfOrder>(game);
                    g.OrderId = model.Id;
                    g.CreatedDate = DateTime.UtcNow.AddHours(7);
                    _context.GameOfOrders.Add(g);
                    if (await _context.SaveChangesAsync() < 0) {
                        return null;
                    }
                }
                toUser.Status = UserStatusConstants.Processing;
                user.Status = UserStatusConstants.Processing;

                await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                    toUserId,
                    $"{user.Name} đã gửi lời mời đến bạn! ",
                    $"{request.Message} ",
                    $"{ValueConstants.BaseUrl}/v1/users/{toUserId}/orders/{model.Id}"));

                if ((await _context.SaveChangesAsync() < 0)) {
                    return null;
                }
                await _context.Entry(model)
                    .Reference(x => x.User)
                    .LoadAsync();

                await _context.Entry(model)
                    .Collection(x => x.GameOfOrders)
                    .Query()
                    .Include(x => x.Game)
                    .LoadAsync();


                var response = _mapper.Map<OrderGetResponse>(model);
                response.ToUser = new OrderUserResponse {
                    Id = toUser.Id,
                    Name = toUser.Name,
                    Avatar = toUser.Avatar,
                    IsActive = toUser.IsActive,
                    IsPlayer = toUser.IsPlayer,
                    Status = toUser.Status
                };
                return response;
            }
            return null;
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderRequestsAsync(
            ClaimsPrincipal principal,
            UserOrderParameter param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var toUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (toUser is null) {
                return null;
            }

            var orderRequests = await _context.Orders.Where(x => x.ToUserId == toUser.Id).ToListAsync();
            var query = orderRequests.AsQueryable();

            FilterOrderByStatus(ref query, param.Status);
            FilterOrderRecent(ref query, param.IsNew);

            orderRequests = query.ToList();

            foreach (var order in orderRequests) {
                await _context.Entry(order)
                    .Reference(x => x.User)
                    .LoadAsync();
                await _context.Entry(order)
                    .Collection(x => x.GameOfOrders)
                    .Query()
                    .Include(x => x.Game)
                    .LoadAsync();
            }

            var responses = _mapper.Map<List<OrderGetResponse>>(orderRequests);
            foreach (var response in responses) {
                response.ToUser = _mapper.Map<OrderUserResponse>(toUser);
            }
            return PagedResult<OrderGetResponse>
                .ToPagedList(
                    responses,
                    param.PageNumber,
                    param.PageSize);
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrdersAsync(
            ClaimsPrincipal principal,
            UserOrderParameter param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return null;
            }

            var orders = await _context.Orders.Where(x => x.UserId == user.Id).ToListAsync();
            var query = orders.AsQueryable();

            FilterOrderByStatus(ref query, param.Status);
            FilterOrderRecent(ref query, param.IsNew);

            orders = query.ToList();

            foreach (var order in orders) {
                await _context.Entry(order)
                    .Reference(x => x.User)
                    .LoadAsync();
                await _context.Entry(order)
                    .Collection(x => x.GameOfOrders)
                    .Query()
                    .Include(x => x.Game)
                    .LoadAsync();
            }

            var responses = _mapper.Map<List<OrderGetResponse>>(orders);
            foreach (var response in responses) {
                var toUser = await _context.Orders.FindAsync(response.ToUserId);
                response.ToUser = _mapper.Map<OrderUserResponse>(toUser);
            }
            return PagedResult<OrderGetResponse>
                .ToPagedList(
                    responses,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterOrderRecent(
            ref IQueryable<Entities.Order> query,
            bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is true) {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
            else {
                query = query.OrderBy(x => x.CreatedDate);
            }
        }

        private void FilterOrderByStatus(
            ref IQueryable<Entities.Order> query,
            string status)
        {
            if (!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)) {
                return;
            }
            query = query.Where(x => x.Status == status);
        }

        public async Task<OrderGetResponse> GetOrderByIdAsync(ClaimsPrincipal principal, string orderId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var order = await _context.Orders.FindAsync(orderId);

            if (order is null) {
                return null;
            }

            if (loggedInUser is not null && user is null) {
                // User is admin
            }
            else {
                if (user.Id != order.UserId && user.Id != order.ToUserId) {
                    return null;
                }
            }

            await _context.Entry(order)
               .Reference(x => x.User)
               .LoadAsync();

            await _context.Entry(order)
                .Collection(x => x.GameOfOrders)
                .Query()
                .Include(x => x.Game)
                .LoadAsync();

            var response = _mapper.Map<OrderGetResponse>(order);
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);

            response.ToUser = new OrderUserResponse {
                Id = toUser.Id,
                Name = toUser.Name,
                Avatar = toUser.Avatar,
                IsActive = toUser.IsActive,
                IsPlayer = toUser.IsPlayer,
                Status = toUser.Status
            };

            return response;
        }

        public async Task<bool> CancelOrderAsync(
            string orderId,
            ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var hirer = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (hirer is null || hirer.Status is not UserStatusConstants.Processing) {
                return false;
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order is null || order.Status is not OrderStatusConstants.Processing) {
                return false;
            }

            await _context.Entry(order)
               .Reference(x => x.User)
               .LoadAsync();

            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);

            toUser.Status = UserStatusConstants.Online;
            order.User.Status = UserStatusConstants.Online;

            if (DateTime.UtcNow.AddHours(7) >= order.ProcessExpired) {
                order.Status = OrderStatusConstants.OverTime; // change status of Order
                await _context.Notifications.AddAsync(
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"Bạn đã bỏ lỡ 1 đề nghị từ {order.User.Name}", $"Bạn đã bỏ lỡ 1 yêu cầu từ {order.User.Name} lúc {order.CreatedDate}", "")
                );
            }
            else {
                order.Status = OrderStatusConstants.Cancel; // change status of Order
                await _context.Notifications.AddAsync(
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"Yêu cầu đã bị Hủy bời {order.User.Name}", $"Yêu cầu đã bị Hủy bời {order.User.Name} lúc {DateTime.UtcNow.AddHours(7)}", "")
                );
            }

            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<bool> ProcessOrderAsync(
            string orderId,
            ClaimsPrincipal principal,
            OrderProcessByPlayerRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var toUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (toUser is null
                || toUser.IsActive is false
                || toUser.Status is not UserStatusConstants.Processing) {
                return false;
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order is null
                || order.Status is not OrderStatusConstants.Processing
                || order.ToUserId != toUser.Id) {
                return false;
            }

            var fromUser = await _context.AppUsers.FindAsync(order.UserId);
            await _context.Entry(fromUser)
                .Reference(x => x.UserBalance)
                .LoadAsync();

            await _context.Entry(order)
               .Reference(x => x.User)
               .Query()
               .Include(x => x.UserBalance)
               .LoadAsync();

            await _context.Entry(toUser)
                .Reference(x => x.UserBalance)
                .LoadAsync();

            if (request.IsAccept == false) {
                order.Status = OrderStatusConstants.Reject;
                order.User.Status = UserStatusConstants.Online;

                toUser.Status = UserStatusConstants.Online;

                await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                    order.UserId,
                    $"{toUser.Name} đã từ chối đề nghị!",
                    $"Xin lỗi vì không thể ghép được với bạn. Mong có thể ghép với bạn vào lần sau!",
                    ""));
            }
            else {
                fromUser.UserBalance.Balance = fromUser.UserBalance.Balance - order.TotalPrices;
                fromUser.UserBalance.ActiveBalance = fromUser.UserBalance.ActiveBalance - order.TotalPrices;

                toUser.Status = UserStatusConstants.Hiring;

                order.Status = OrderStatusConstants.Start;
                order.TimeStart = DateTime.UtcNow.AddHours(7);
                order.User.Status = UserStatusConstants.Hiring;
            }

            if ((await _context.SaveChangesAsync() >= 0)) {
                await _context.TransactionHistories.AddAsync(
                    Helpers.TransactionHelpers.PopulateTransactionHistory(
                        order.User.UserBalance.Id,
                        "-",
                        order.TotalPrices,
                        "Order",
                        orderId)
                );
                return (await _context.SaveChangesAsync() >= 0);
            }
            return false;
        }

        public async Task<bool> FinishOrderAsync(string orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order is null) {
                return false;
            }

            if (order.Status is not OrderStatusConstants.Start) {
                return false;
            }

            if (DateTime.UtcNow.AddHours(7) < order.TimeStart.AddHours(order.TotalTimes)) {
                return false;
            }

            await _context.Entry(order)
                          .Reference(x => x.User)
                          .Query()
                          .Include(x => x.UserBalance)
                          .LoadAsync();
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
            await _context.Entry(toUser)
                          .Reference(x => x.UserBalance)
                          .LoadAsync();

            order.Status = OrderStatusConstants.Finish;
            order.User.Status = UserStatusConstants.Online;
            toUser.Status = UserStatusConstants.Online;

            order.TimeFinish = DateTime.UtcNow.AddHours(7);
            order.FinalPrices = order.TotalPrices;

            if ((await _context.SaveChangesAsync() >= 0)) {
                toUser.UserBalance.Balance += order.TotalPrices;
                await _context.TransactionHistories.AddAsync(
                    Helpers.TransactionHelpers.PopulateTransactionHistory(
                        toUser.UserBalance.Id,
                        "+",
                        order.FinalPrices,
                        "Order",
                        orderId)
                );

                await _context.UnActiveBalances.AddAsync(
                    Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                        toUser.UserBalance.Id,
                        orderId,
                        order.FinalPrices,
                        DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney))
                );
                return (await _context.SaveChangesAsync() >= 0);
            }
            return false;
        }

        public async Task<bool> FinishOrderSoonAsync(
            string orderId,
            ClaimsPrincipal principal,
            FinishSoonRequest request)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order is null) {
                return false;
            }

            if (order.Status is not OrderStatusConstants.Start) {
                return false;
            }

            if (DateTime.UtcNow.AddHours(7) > order.TimeStart.AddHours(order.TotalTimes)) {
                return false;
            }

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            await _context.Entry(order)
                          .Reference(x => x.User)
                          .Query()
                          .Include(x => x.UserBalance)
                          .LoadAsync();
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
            await _context.Entry(toUser)
                          .Reference(x => x.UserBalance)
                          .LoadAsync();

            order.Status = OrderStatusConstants.FinishSoon;
            order.User.Status = UserStatusConstants.Online;
            toUser.Status = UserStatusConstants.Online;
            order.TimeFinish = DateTime.UtcNow.AddHours(7);

            if (order.User.IdentityId == identityId) {
                await _context.Notifications.AddAsync(
                    Helpers.NotificationHelpers.PopulateNotification(
                        toUser.Id,
                        $"{order.User.Name} đã yêu cầu kết thúc sớm",
                        (String.IsNullOrEmpty(request.Message) || String.IsNullOrWhiteSpace(request.Message)) ? $"Yêu cầu đã kết thúc lúc {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} đã yêu cầu kết thúc sớm với lời nhắn: {request.Message}. Yêu cầu đã kết thúc lúc {DateTime.UtcNow.AddHours(7)}",
                        "")
                );
            }
            else {
                await _context.Notifications.AddAsync(
                    Helpers.NotificationHelpers.PopulateNotification(
                        order.UserId,
                        $"{toUser.Name} đã yêu cầu kết thúc sớm",
                        (String.IsNullOrEmpty(request.Message) || String.IsNullOrWhiteSpace(request.Message)) ? $"Yêu cầu đã kết thúc lúc {DateTime.UtcNow.AddHours(7)}" : $"{toUser.Name} đã yêu cầu kết thúc sớm với lời nhắn: {request.Message}. Yêu cầu đã kết thúc lúc {DateTime.UtcNow.AddHours(7)}",
                        "")
                );
            }
            var priceDone = (order.TotalPrices * GetTimeDone(order.TimeStart)) / (order.TotalTimes * 60 * 60);
            order.FinalPrices = ((float)priceDone);

            if ((await _context.SaveChangesAsync() >= 0)) {
                toUser.UserBalance.Balance += order.FinalPrices;
                order.User.UserBalance.Balance += (order.TotalPrices - order.FinalPrices);
                order.User.UserBalance.ActiveBalance += (order.TotalPrices - order.FinalPrices);

                await _context.TransactionHistories.AddRangeAsync(
                    Helpers.TransactionHelpers.PopulateTransactionHistory(
                        order.User.UserBalance.Id,
                        "+",
                        (order.TotalPrices - order.FinalPrices),
                        "Order",
                        orderId)
                    ,
                    Helpers.TransactionHelpers.PopulateTransactionHistory(
                        toUser.UserBalance.Id,
                        "+",
                        order.FinalPrices,
                        "Order",
                        orderId)
                );

                await _context.UnActiveBalances.AddAsync(
                    Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                        toUser.UserBalance.Id,
                        orderId,
                        order.FinalPrices,
                        DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney))
                );
                return (await _context.SaveChangesAsync() >= 0);
            }
            return false;
        }
        public double GetTimeDone(DateTime date)
        {
            TimeSpan ts = DateTime.UtcNow.AddHours(7) - date;
            var timeDone = ts.TotalSeconds;
            return timeDone;
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(
            string userId,
            AdminOrderParameters param)
        {
            var orders = await _context.Orders.Where(x => (x.UserId + x.ToUserId).Contains(userId))
                                              .ToListAsync();

            var query = orders.AsQueryable();
            FilterInDate(ref query, param.FromDate, param.ToDate);
            FilterOrderByStatus(ref query, param.Status);
            FilterOrderRecent(ref query, param.IsNew);
            orders = query.ToList();

            foreach (var order in orders) {
                await _context.Entry(order)
                    .Reference(x => x.User)
                    .LoadAsync();
                await _context.Entry(order)
                    .Collection(x => x.GameOfOrders)
                    .Query()
                    .Include(x => x.Game)
                    .LoadAsync();
            }

            var responses = _mapper.Map<List<OrderGetResponse>>(orders);
            foreach (var response in responses) {
                var toUser = await _context.Orders.FindAsync(response.ToUserId);
                response.ToUser = _mapper.Map<OrderUserResponse>(toUser);
            }
            return PagedResult<OrderGetResponse>
                .ToPagedList(
                    responses,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterInDate(
            ref IQueryable<Entities.Order> query,
            DateTime? fromDate,
            DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate
            && x.CreatedDate <= toDate);
        }

        public async Task<OrderGetDetailResponse> GetOrderByIdInDetailAsync(string orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null) {
                return null;
            }

            await _context.Entry(order)
               .Reference(x => x.User)
               .LoadAsync();

            await _context.Entry(order)
                .Collection(x => x.Ratings)
                .LoadAsync();

            await _context.Entry(order)
                .Collection(x => x.Reports)
                .LoadAsync();

            await _context.Entry(order)
                    .Collection(x => x.GameOfOrders)
                    .Query()
                    .Include(x => x.Game)
                    .LoadAsync();

            var response = _mapper.Map<OrderGetDetailResponse>(order);

            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);

            response.ToUser = new OrderUserResponse {
                Id = toUser.Id,
                Name = toUser.Name,
                Avatar = toUser.Avatar,
                IsActive = toUser.IsActive,
                IsPlayer = toUser.IsPlayer,
                Status = toUser.Status
            };

            return response;
        }
    }
}