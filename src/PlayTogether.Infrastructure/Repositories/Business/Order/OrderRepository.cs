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

        public async Task<Result<OrderGetResponse>> CreateOrderAsync(
            ClaimsPrincipal principal,
            string toUserId,
            OrderCreateRequest request)
        {
            var result = new Result<OrderGetResponse>();

            // Check hirer
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (user.Id == toUserId) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "B???n kh??ng th??? thu?? ch??nh b???n ???????c.");
                return result;
            }

            if (user.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Disable);
                return result;
            }

            if (user.IsPlayer is true) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "B???n ??ang b???t ch??? ????? nh???n thu??. Vui l??ng t???t ??i v?? th??? l???i.");
                return result;
            }

            if (user.Status is not UserStatusConstants.Online) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotReady + $" Hi???n t??i kho???n b???n ??ang ??? ch??? ????? {user.Status}. Vui l??ng th??? l???i sau khi t???t c??? c??c thao t??c ho??n t???t.");
                return result;
            }

            // Check player and status of player
            var toUser = await _context.AppUsers.FindAsync(toUserId);
            if (toUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y ng?????i b???n mu???n thu??.");
                return result;
            }

            if (toUser.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Player{toUser.Name} hi???n ??ang b??? kh??a t??i kho???n. Vui l??ng th??? l???i sau.");
                return result;
            }

            // only online, offline can be receive order
            if (toUser.Status is not UserStatusConstants.Online && toUser.Status is not UserStatusConstants.Offline) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Player{toUser.Name} hi???n kh??ng s???n s??ng nh???n thu??. Vui l??ng th??? l???i sau.");
                return result;
            }

            if (toUser.IsPlayer is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Player{toUser.Name} hi???n ??ang kh??ng nh???n thu??. Vui l??ng th??? l???i sau.");
                return result;
            }

            await _context.Entry(user).Reference(x => x.UserBalance).LoadAsync();
            await _context.Entry(toUser).Reference(x => x.UserBalance).LoadAsync();

            if (request.TotalTimes > toUser.MaxHourHire) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Player{toUser.Name} ch??? nh???n thu?? t???i ??a {toUser.MaxHourHire} gi??? m???i l?????t thu??. Vui l??ng th??? l???i sau.");
                return result;
            }

            if (request.TotalTimes < 1) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"B???n vui l??ng nh???p gi??? thu?? h???p l??, Player nh???n thu?? v???i th???i gian ??t nh???t l?? 1 gi???. Vui l??ng nh???p l???i.");
                return result;
            }

            // Check balance of hirer
            if ((request.TotalTimes * toUser.PricePerHour) > user.UserBalance.ActiveBalance) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"T??i kho???n active c???a b???n hi???n l?? {user.UserBalance.ActiveBalance} hi???n ??ang kh??ng ????? ????? thu?? Player {toUser.Name}. Vui l??ng n???p ti???n v?? quay l???i.");
                return result;
            }

            if (request.Games.Count == 0) {
                return null;
            }

            IEnumerable<GamesOfOrderCreateRequest> duplicates = request.Games.GroupBy(x => x)
                                        .SelectMany(g => g.Skip(1));
            if (duplicates.Count() > 0) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"B???n ch??a nh???p game m?? b???n mu???n thu?? ch??i c??ng. Vui l??ng nh???p game v?? quay l???i.");
                return result;
            }

            foreach (var game in request.Games) {
                var isSkill = await _context.GameOfUsers.Where(x => x.UserId == toUserId)
                                                            .AnyAsync(x => x.GameId == game.GameId);
                if (!isSkill) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Game b???n ch???n kh??ng ph???i l?? game m?? Player h??? tr???. Vui l??ng nh???p l???i game ho???c t??m Player kh??c ph?? h???p.");
                    return result;
                }
            }

            var processTime = await _context.SystemConfigs.FirstOrDefaultAsync(x => x.NO == 1);
            if (processTime is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y c???u h??nh th???i gian ch??? x??? l?? y??u c???u thu??. Vui l??ng th??ng b??o t???i qu???n tr??? vi??n. Xin ch??n th??nh c???m ??n.");
                return result;
            }
            var model = _mapper.Map<Core.Entities.Order>(request);
            model.ToUserId = toUserId;
            model.UserId = user.Id;
            model.TotalPrices = request.TotalTimes * toUser.PricePerHour;
            model.Status = OrderStatusConstants.Processing;
            model.ProcessExpired = DateTime.UtcNow.AddHours(7).AddMinutes(processTime.Value);
            // model.ProcessExpired = DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.OrderProcessExpireTime);
            // model.ProcessExpired = DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.OrderProcessExpireTimeForTest);

            _context.Orders.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                foreach (var game in request.Games) {
                    var existGame = await _context.GameOfOrders.Where(x => x.OrderId == model.Id)
                                                                .AnyAsync(x => x.GameId == game.GameId);
                    if (existGame) continue;
                    var g = _mapper.Map<Core.Entities.GameOfOrder>(game);
                    g.OrderId = model.Id;
                    g.CreatedDate = DateTime.UtcNow.AddHours(7);
                    _context.GameOfOrders.Add(g);
                    if (await _context.SaveChangesAsync() < 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                        return result;
                    }
                }
                toUser.Status = UserStatusConstants.Processing;
                user.Status = UserStatusConstants.Processing;

                await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                    toUserId,
                    $"{user.Name} ???? g???i l???i m???i ?????n b???n! ",
                    $"{request.Message} ",
                    $"{ValueConstants.BaseUrl}/v1/users/{toUserId}/orders/{model.Id}"));

                if ((await _context.SaveChangesAsync() < 0)) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
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
                    Email = toUser.Email,
                    Avatar = toUser.Avatar,
                    IsActive = toUser.IsActive,
                    IsPlayer = toUser.IsPlayer,
                    Status = toUser.Status
                };
                result.Content = response;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<OrderGetResponse>> GetAllOrderRequestsAsync(
            ClaimsPrincipal principal,
            UserOrderParameter param)
        {
            var result = new PagedResult<OrderGetResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var toUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (toUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
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
            var result = new PagedResult<OrderGetResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
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
                var toUser = await _context.AppUsers.FindAsync(response.ToUserId);
                response.ToUser = _mapper.Map<OrderUserResponse>(toUser);
            }
            return PagedResult<OrderGetResponse>
                .ToPagedList(
                    responses,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterOrderRecent(
            ref IQueryable<Core.Entities.Order> query,
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
            ref IQueryable<Core.Entities.Order> query,
            string status)
        {
            if (!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)) {
                return;
            }
            query = query.Where(x => x.Status.ToLower().Contains(status.ToLower()));
        }

        public async Task<Result<OrderGetResponse>> GetOrderByIdAsync(ClaimsPrincipal principal, string orderId)
        {
            var result = new Result<OrderGetResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var order = await _context.Orders.FindAsync(orderId);

            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order n??y.");
                return result;
            }

            if (loggedInUser is not null && user is null) {
                // User is admin
            }
            else {
                if (user.Id != order.UserId && user.Id != order.ToUserId) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                    return result;
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
                Email = toUser.Email,
                Avatar = toUser.Avatar,
                IsActive = toUser.IsActive,
                IsPlayer = toUser.IsPlayer,
                Status = toUser.Status
            };
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> CancelOrderAsync(
            string orderId, ClaimsPrincipal principal, OrderCancelRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var hirer = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (hirer is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            if (hirer.Status is not UserStatusConstants.Processing) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Ng?????i d??ng kh??ng th??? h???y order request n??y ???????c.");
                return result;
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order n??y.");
                return result;
            }

            if (order.UserId != hirer.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order n??y kh??ng thu???c v??? b???n.");
                return result;
            }

            if (order.Status is not OrderStatusConstants.Processing) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order request n??y kh??ng th??? h???y ???????c.");
                return result;
            }

            await _context.Entry(order)
               .Reference(x => x.User)
               .LoadAsync();

            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
            await _context.Entry(toUser).Reference(x => x.BehaviorPoint).LoadAsync();

            toUser.Status = UserStatusConstants.Online;
            order.User.Status = UserStatusConstants.Online;
            await _context.SaveChangesAsync();

            if (DateTime.UtcNow.AddHours(7) >= order.ProcessExpired) {
                order.Status = OrderStatusConstants.OverTime; // change status of Order
                toUser.IsPlayer = false;
                toUser.BehaviorPoint.Point -= 1;
                toUser.BehaviorPoint.SatisfiedPoint -= 2;
                await _context.Notifications.AddRangeAsync(
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"B???n ???? b??? l??? 1 ????? ngh??? t??? {order.User.Name}", $"B???n ???? b??? l??? 1 y??u c???u t??? {order.User.Name} l??c {order.CreatedDate}. B???n s??? b??? tr??? 1 ??i???m uy t??n, 2 ??i???m t??ch c???c. Mong b???n ch?? ??.", ""),
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"Ch??? ????? nh???n thu?? ???? t???t!!!", $"C?? l??? b???n kh??ng th???t s??? Online. H??? th???ng ???? t??? ?????ng t???t ch??? ????? nh???n thu??!!!", "")
                );
                await _context.BehaviorHistories.AddRangeAsync(
                    Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.OrderOverTime, 1, BehaviorTypeConstants.Point, orderId),
                    Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.OrderOverTime, 2, BehaviorTypeConstants.SatisfiedPoint, orderId)
                );
            }
            else {
                order.Status = OrderStatusConstants.Cancel; // change status of Order
                order.Reason = request.Reason;
                await _context.Notifications.AddAsync(
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"Y??u c???u ???? b??? H???y b???i {order.User.Name}", $"Y??u c???u ???? b??? H???y b???i {order.User.Name} l??c {DateTime.UtcNow.AddHours(7)}", "")
                );
            }

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> ProcessOrderAsync(
            string orderId,
            ClaimsPrincipal principal,
            OrderProcessByPlayerRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var toUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            if (toUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.Unauthenticate);
                return result;
            }

            if (toUser.IsActive is false) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Player{toUser.Name} hi???n ??ang b??? kh??a t??i kho???n. Vui l??ng th??? l???i sau.");
                return result;
            }

            if (toUser.Status is not UserStatusConstants.Processing) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"B???n hi???n ??ang kh??ng c?? b???t k?? order request n??o.");
                return result;
            }

            var order = await _context.Orders.FindAsync(orderId);
            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order n??y.");
                return result;
            }

            if (order.Status is not OrderStatusConstants.Processing) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order request n??y kh??ng th??? x??? l??.");
                return result;
            }

            if (order.ToUserId != toUser.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order request n??y kh??ng thu???c v??? b???n.");
                return result;
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

            await _context.Entry(toUser)
                .Reference(x => x.BehaviorPoint)
                .LoadAsync();

            if (DateTime.UtcNow.AddHours(7) >= order.ProcessExpired) {
                order.Status = OrderStatusConstants.OverTime;
                fromUser.Status = UserStatusConstants.Online;
                toUser.Status = UserStatusConstants.Online;
                toUser.IsPlayer = false;
                toUser.BehaviorPoint.Point -= 1;
                toUser.BehaviorPoint.SatisfiedPoint -= 2;
                await _context.Notifications.AddRangeAsync(
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"B???n ???? b??? l??? 1 ????? ngh??? t??? {order.User.Name}", $"B???n ???? b??? l??? 1 y??u c???u t??? {order.User.Name} l??c {order.CreatedDate}. B???n s??? b??? tr??? 1 ??i???m uy t??n, 2 ??i???m t??ch c???c. Mong b???n ch?? ??.", ""),
                    Helpers.NotificationHelpers.PopulateNotification(order.ToUserId, $"Ch??? ????? nh???n thu?? ???? t???t!!!", $"C?? l??? b???n kh??ng th???t s??? Online. H??? th???ng ???? t??? ?????ng t???t ch??? ????? nh???n thu??!!!", "")
                );
                await _context.BehaviorHistories.AddRangeAsync(
                    Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.OrderOverTime, 1, BehaviorTypeConstants.Point, orderId),
                    Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.OrderOverTime, 2, BehaviorTypeConstants.SatisfiedPoint, orderId)
                );
            }
            else {
                if (request.IsAccept == false) {
                    order.Status = OrderStatusConstants.Reject;
                    order.User.Status = UserStatusConstants.Online;

                    toUser.Status = UserStatusConstants.Online;
                    order.Reason = request.Reason;

                    toUser.BehaviorPoint.SatisfiedPoint -= 1;
                    await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.OrderReject, 1, BehaviorTypeConstants.SatisfiedPoint, orderId));

                    await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                        order.UserId,
                        $"{toUser.Name} ???? t??? ch???i ????? ngh???!",
                        $"Xin l???i v?? kh??ng th??? gh??p ???????c v???i b???n, v?? {request.Reason}. Mong c?? th??? gh??p v???i b???n v??o l???n sau!",
                        ""));
                }
                else {
                    toUser.NumOfOrder += 1;
                    fromUser.UserBalance.Balance = fromUser.UserBalance.Balance - order.TotalPrices;
                    fromUser.UserBalance.ActiveBalance = fromUser.UserBalance.ActiveBalance - order.TotalPrices;

                    await _context.TransactionHistories.AddAsync(
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            fromUser.UserBalance.Id,
                            TransactionTypeConstants.Sub,
                            order.TotalPrices,
                            TransactionTypeConstants.Order,
                            orderId)
                    );

                    toUser.Status = UserStatusConstants.Hiring;

                    order.Status = OrderStatusConstants.Start;
                    order.TimeStart = DateTime.UtcNow.AddHours(7);

                    fromUser.Status = UserStatusConstants.Hiring;
                }
            }



            if ((await _context.SaveChangesAsync() >= 0)) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> FinishOrderAsync(string orderId)
        {
            var result = new Result<bool>();
            var order = await _context.Orders.FindAsync(orderId);
            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order n??y.");
                return result;
            }

            if (order.Status is not OrderStatusConstants.Start) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order n??y ch??a b???t ?????u.");
                return result;
            }

            if (DateTime.UtcNow.AddHours(7) < order.TimeStart.AddHours(order.TotalTimes)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order n??y ch??a h???t gi???.");
                return result;
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

            order.Status = OrderStatusConstants.Complete;
            order.User.Status = UserStatusConstants.Online;
            order.Reason = "";
            toUser.Status = UserStatusConstants.Online;
            toUser.NumOfFinishOnTime += 1;
            toUser.TotalTimeOrder += order.TotalTimes;

            order.TimeFinish = DateTime.UtcNow.AddHours(7);

            var percentMoneyPayForSystem = await _context.SystemConfigs.FirstOrDefaultAsync(x => x.NO == 4);
            if (percentMoneyPayForSystem is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y c???u h??nh ph???n tr??m ti???n tr??ch cho h??? th???ng. Vui l??ng th??ng b??o t???i qu???n tr??? vi??n. Xin ch??n th??nh c???m ??n.");
                return result;
            }
            order.FinalPrices = order.TotalPrices - (order.TotalPrices * percentMoneyPayForSystem.Value);
            order.PercentSub = percentMoneyPayForSystem.Value;

            var moneyActiveTime = await _context.SystemConfigs.FirstOrDefaultAsync(x => x.NO == 2);
            if (moneyActiveTime is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y c???u h??nh th???i gian ch??? k??ch ho???t ti???n. Vui l??ng th??ng b??o t???i qu???n tr??? vi??n. Xin ch??n th??nh c???m ??n.");
                return result;
            }

            if ((await _context.SaveChangesAsync() >= 0)) {
                toUser.UserBalance.Balance += order.FinalPrices;
                await _context.TransactionHistories.AddAsync(
                    Helpers.TransactionHelpers.PopulateTransactionHistory(
                        toUser.UserBalance.Id,
                        TransactionTypeConstants.Add,
                        order.FinalPrices,
                        TransactionTypeConstants.Order,
                        orderId)
                );

                await _context.UnActiveBalances.AddAsync(
                    Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                        toUser.UserBalance.Id,
                        orderId,
                        order.FinalPrices,
                        DateTime.UtcNow.AddHours(7).AddHours(moneyActiveTime.Value)
                        // DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney)
                        // DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.HourActiveMoneyForTest)
                        )
                );
                if (await _context.SaveChangesAsync() >= 0) {
                    result.Content = true;
                    return result;
                }
                result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> FinishOrderSoonAsync(
            string orderId,
            ClaimsPrincipal principal,
            FinishSoonRequest request)
        {
            var result = new Result<bool>();
            var order = await _context.Orders.FindAsync(orderId);
            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order n??y.");
                return result;
            }

            if (order.Status is not OrderStatusConstants.Start) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order n??y ch??a b???t ?????u.");
                return result;
            }

            if (DateTime.UtcNow.AddHours(7) > order.TimeStart.AddHours(order.TotalTimes)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Order n??y ???? h???t gi???. Vui l??ng ch??? h??? th???ng k???t th??c.");
                return result;
            }

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
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
            await _context.Entry(toUser)
                            .Reference(x => x.BehaviorPoint)
                            .LoadAsync();


            order.User.Status = UserStatusConstants.Online;
            toUser.Status = UserStatusConstants.Online;

            order.TimeFinish = DateTime.UtcNow.AddHours(7);
            order.Reason = request.Reason;

            var percentMoneyPayForSystem = await _context.SystemConfigs.FirstOrDefaultAsync(x => x.NO == 4);
            if (percentMoneyPayForSystem is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y c???u h??nh ph???n tr??m ti???n tr??ch cho h??? th???ng. Vui l??ng th??ng b??o t???i qu???n tr??? vi??n. Xin ch??n th??nh c???m ??n.");
                return result;
            }

            // var priceDone = (order.TotalPrices * Helpers.UtilsHelpers.GetTimeDone(order.TimeStart)) / (order.TotalTimes * 60 * 60);
            var priceDone = CalculateMoneyFinish(order.TotalTimes * 3600, order.TotalPrices, Helpers.UtilsHelpers.GetTimeDone(order.TimeStart));
            order.FinalPrices = ((float)priceDone.Item1) - (((float)priceDone.Item1) * percentMoneyPayForSystem.Value);
            order.PercentSub = percentMoneyPayForSystem.Value;

            var moneyActiveTime = await _context.SystemConfigs.FirstOrDefaultAsync(x => x.NO == 2);
            if (moneyActiveTime is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Kh??ng t??m th???y c???u h??nh th???i gian ch??? k??ch ho???t ti???n. Vui l??ng th??ng b??o t???i qu???n tr??? vi??n. Xin ch??n th??nh c???m ??n.");
                return result;
            }
            toUser.TotalTimeOrder += Convert.ToInt32(Math.Ceiling(Helpers.UtilsHelpers.GetTimeDone(order.TimeStart) / 3600));
            if (order.User.IdentityId == identityId) { // Hirer finish soon
                order.Reason = request.Reason;
                if (priceDone.Item2 == 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "C?? l???i x???y ra kh??ng k???t th??c order s???m ???????c.");
                    return result;
                }
                else if (priceDone.Item2 == 1) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "B???n ch??? c?? th??? k???t th??c sau khi ho??n th??nh 1/10 th???i gian thu??.");
                    return result;

                }
                else if (priceDone.Item2 == 10) {
                    await _context.Notifications.AddRangeAsync(
                        Helpers.NotificationHelpers.PopulateNotification(
                            toUser.Id,
                            $"{order.User.Name} ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}. Do order ???? ho??n th??nh h??n 90% th???i l?????ng n??n s??? ???????c t??nh l?? order ???? ho??n th??nh.",
                            ""),
                        Helpers.NotificationHelpers.PopulateNotification(
                            order.User.Id,
                            $"B???n ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}. Do order ???? ho??n th??nh h??n 90% th???i l?????ng n??n s??? ???????c t??nh l?? order ???? ho??n th??nh.",
                            "")
                    );
                    toUser.UserBalance.Balance += order.FinalPrices;
                    await _context.TransactionHistories.AddRangeAsync(
                        // Helpers.TransactionHelpers.PopulateTransactionHistory(
                        //     order.User.UserBalance.Id,
                        //     TransactionTypeConstants.Sub,
                        //     order.TotalPrices,
                        //     TransactionTypeConstants.OrderRefund,
                        //     orderId)
                        // ,
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            toUser.UserBalance.Id,
                            TransactionTypeConstants.Add,
                            order.FinalPrices,
                            TransactionTypeConstants.Order,
                            orderId)
                    );

                    await _context.UnActiveBalances.AddAsync(
                        Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                            toUser.UserBalance.Id,
                            orderId,
                            order.FinalPrices,
                            DateTime.UtcNow.AddHours(7).AddHours(moneyActiveTime.Value)
                            // DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney)
                            // DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.HourActiveMoneyForTest)
                            )
                    );
                    order.Status = OrderStatusConstants.Complete;
                    order.User.Status = UserStatusConstants.Online;
                    toUser.Status = UserStatusConstants.Online;
                    if (await _context.SaveChangesAsync() >= 0) {
                        result.Content = true;
                        return result;
                    }
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;

                }
                else {
                    await _context.Notifications.AddRangeAsync(
                        Helpers.NotificationHelpers.PopulateNotification(
                            toUser.Id,
                            $"{order.User.Name} ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}.",
                            ""),
                        Helpers.NotificationHelpers.PopulateNotification(
                            order.User.Id,
                            $"B???n ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}.",
                            "")
                    );
                    toUser.UserBalance.Balance += order.FinalPrices;
                    order.User.UserBalance.Balance += (order.TotalPrices - ((float)priceDone.Item1));
                    order.User.UserBalance.ActiveBalance += (order.TotalPrices - ((float)priceDone.Item1));
                    await _context.TransactionHistories.AddRangeAsync(
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            order.User.UserBalance.Id,
                            TransactionTypeConstants.Add,
                            (order.TotalPrices - ((float)priceDone.Item1)),
                            TransactionTypeConstants.OrderRefund,
                            orderId)
                        ,
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            toUser.UserBalance.Id,
                            TransactionTypeConstants.Add,
                            order.FinalPrices,
                            TransactionTypeConstants.Order,
                            orderId)
                    );

                    await _context.UnActiveBalances.AddAsync(
                        Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                            toUser.UserBalance.Id,
                            orderId,
                            order.FinalPrices,
                            DateTime.UtcNow.AddHours(7).AddHours(moneyActiveTime.Value)
                            // DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney)
                            // DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.HourActiveMoneyForTest)
                            )
                    );
                    order.Status = OrderStatusConstants.FinishSoonHirer;
                    order.User.Status = UserStatusConstants.Online;
                    toUser.Status = UserStatusConstants.Online;
                    if (await _context.SaveChangesAsync() >= 0) {
                        result.Content = true;
                        return result;
                    }
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }
            else { // Player finish soon
                order.Reason = request.Reason;
                if (priceDone.Item2 == 0) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "C?? l???i x???y ra kh??ng k???t th??c order s???m ???????c.");
                    return result;
                }
                else if (priceDone.Item2 == 1) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "B???n ch??? c?? th??? k???t th??c sau khi ho??n th??nh 1/10 th???i gian thu??.");
                    return result;

                }
                else if (priceDone.Item2 == 10) {
                    await _context.Notifications.AddRangeAsync(
                        Helpers.NotificationHelpers.PopulateNotification(
                            order.User.Id,
                            $"{toUser.Name} ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}. Do order ???? ho??n th??nh h??n 90% th???i l?????ng n??n s??? ???????c t??nh l?? order ???? ho??n th??nh.",
                            ""),
                        Helpers.NotificationHelpers.PopulateNotification(
                            toUser.Id,
                            $"B???n ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}. Do order ???? ho??n th??nh h??n 90% th???i l?????ng n??n s??? ???????c t??nh l?? order ???? ho??n th??nh.",
                            "")
                    );
                    toUser.UserBalance.Balance += order.FinalPrices;
                    await _context.TransactionHistories.AddRangeAsync(
                        // Helpers.TransactionHelpers.PopulateTransactionHistory(
                        //     order.User.UserBalance.Id,
                        //     TransactionTypeConstants.Sub,
                        //     order.TotalPrices,
                        //     TransactionTypeConstants.OrderRefund,
                        //     orderId)
                        // ,
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            toUser.UserBalance.Id,
                            TransactionTypeConstants.Add,
                            order.FinalPrices,
                            TransactionTypeConstants.Order,
                            orderId)
                    );

                    await _context.UnActiveBalances.AddAsync(
                        Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                            toUser.UserBalance.Id,
                            orderId,
                            order.FinalPrices,
                            DateTime.UtcNow.AddHours(7).AddHours(moneyActiveTime.Value)
                            // DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney)
                            // DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.HourActiveMoneyForTest)
                            )
                    );
                    order.Status = OrderStatusConstants.Complete;
                    order.User.Status = UserStatusConstants.Online;
                    toUser.Status = UserStatusConstants.Online;
                    if (await _context.SaveChangesAsync() >= 0) {
                        result.Content = true;
                        return result;
                    }
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;

                }
                else {
                    await _context.Notifications.AddRangeAsync(
                        Helpers.NotificationHelpers.PopulateNotification(
                            order.User.Id,
                            $"{toUser.Name} ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}.",
                            ""),
                        Helpers.NotificationHelpers.PopulateNotification(
                            toUser.Id,
                            $"B???n ???? y??u c???u k???t th??c s???m",
                            (String.IsNullOrEmpty(request.Reason) || String.IsNullOrWhiteSpace(request.Reason)) ? $"Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}" : $"{order.User.Name} ???? y??u c???u k???t th??c s???m v???i l???i nh???n: {request.Reason}. Y??u c???u ???? k???t th??c l??c {DateTime.UtcNow.AddHours(7)}. B???n b??? tr??? {10 - priceDone.Item2} ??i???m t??ch c???c v?? ???? k???t th??c order s???m.",
                            "")
                    );
                    toUser.UserBalance.Balance += order.FinalPrices;
                    order.User.UserBalance.Balance += (order.TotalPrices - ((float)priceDone.Item1));
                    order.User.UserBalance.ActiveBalance += (order.TotalPrices - ((float)priceDone.Item1));
                    await _context.TransactionHistories.AddRangeAsync(
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            order.User.UserBalance.Id,
                            TransactionTypeConstants.Add,
                            (order.TotalPrices - ((float)priceDone.Item1)),
                            TransactionTypeConstants.OrderRefund,
                            orderId)
                        ,
                        Helpers.TransactionHelpers.PopulateTransactionHistory(
                            toUser.UserBalance.Id,
                            TransactionTypeConstants.Add,
                            order.FinalPrices,
                            TransactionTypeConstants.Order,
                            orderId)
                    );

                    await _context.UnActiveBalances.AddAsync(
                        Helpers.UnActiveBalanceHelpers.PopulateUnActiveBalance(
                            toUser.UserBalance.Id,
                            orderId,
                            order.FinalPrices,
                            DateTime.UtcNow.AddHours(7).AddHours(moneyActiveTime.Value)
                            // DateTime.UtcNow.AddHours(7).AddHours(ValueConstants.HourActiveMoney)
                            // DateTime.UtcNow.AddHours(7).AddMinutes(ValueConstants.HourActiveMoneyForTest)
                            )
                    );
                    order.Status = OrderStatusConstants.FinishSoonHirer;
                    order.User.Status = UserStatusConstants.Online;
                    toUser.Status = UserStatusConstants.Online;
                    toUser.BehaviorPoint.SatisfiedPoint -= (10 - priceDone.Item2);
                    await _context.BehaviorHistories.AddRangeAsync(
                        Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.BehaviorPoint.Id,
                            BehaviorTypeConstants.Sub,
                            BehaviorTypeConstants.OrderFinishSoon,
                            (10 - priceDone.Item2),
                            BehaviorTypeConstants.SatisfiedPoint,
                            orderId
                        )
                    );
                    if (await _context.SaveChangesAsync() >= 0) {
                        result.Content = true;
                        return result;
                    }
                    result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                    return result;
                }
            }

        }

        private (float, int) CalculateMoneyFinish(int totalTime, float totalPrice, double timeDone)
        {
            float finalPrice = 0;
            // var percent = timeDone / totalTime;
            if (timeDone <= ((totalTime * 1) / 10)) {
                finalPrice = 0;
                return (finalPrice, 1);
            }
            if (timeDone > ((totalTime * 1) / 10) && timeDone <= ((totalTime * 2) / 10)) {
                finalPrice = totalPrice * 1 / 10;
                return (finalPrice, 2);
            }
            if (timeDone > ((totalTime * 2) / 10) && timeDone <= ((totalTime * 3) / 10)) {
                finalPrice = totalPrice * 2 / 10;
                return (finalPrice, 3);
            }
            if (timeDone > ((totalTime * 3) / 10) && timeDone <= ((totalTime * 4) / 10)) {
                finalPrice = totalPrice * 3 / 10;
                return (finalPrice, 4);
            }
            if (timeDone > ((totalTime * 4) / 10) && timeDone <= ((totalTime * 5) / 10)) {
                finalPrice = totalPrice * 4 / 10;
                return (finalPrice, 5);
            }
            if (timeDone > ((totalTime * 5) / 10) && timeDone <= ((totalTime * 6) / 10)) {
                finalPrice = totalPrice * 5 / 10;
                return (finalPrice, 6);
            }
            if (timeDone > ((totalTime * 6) / 10) && timeDone <= ((totalTime * 7) / 10)) {
                finalPrice = totalPrice * 6 / 10;
                return (finalPrice, 7);
            }
            if (timeDone > ((totalTime * 7) / 10) && timeDone <= ((totalTime * 8) / 10)) {
                finalPrice = totalPrice * 7 / 10;
                return (finalPrice, 8);
            }
            if (timeDone > ((totalTime * 8) / 10) && timeDone <= ((totalTime * 9) / 10)) {
                finalPrice = totalPrice * 8 / 10;
                return (finalPrice, 9);
            }
            if (timeDone > ((totalTime * 9) / 10)) {
                finalPrice = totalPrice;
                return (finalPrice, 10);
            }
            return (0, 0);
        }


        public async Task<PagedResult<OrderGetResponse>> GetAllOrderByUserIdForAdminAsync(
            string userId,
            AdminOrderParameters param)
        {
            var result = new PagedResult<OrderGetResponse>();
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
                var toUser = await _context.AppUsers.FindAsync(response.ToUserId);
                response.ToUser = _mapper.Map<OrderUserResponse>(toUser);
            }
            return PagedResult<OrderGetResponse>
                .ToPagedList(
                    responses,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterInDate(
            ref IQueryable<Core.Entities.Order> query,
            DateTime? fromDate,
            DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate
            && x.CreatedDate <= toDate);
        }

        public async Task<Result<OrderGetDetailResponse>> GetOrderByIdInDetailAsync(string orderId)
        {
            var result = new Result<OrderGetDetailResponse>();
            var order = await _context.Orders.FindAsync(orderId);

            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order n??y.");
                return result;
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
                Email = toUser.Email,
                Avatar = toUser.Avatar,
                IsActive = toUser.IsActive,
                IsPlayer = toUser.IsPlayer,
                Status = toUser.Status
            };

            result.Content = response;
            return result;
        }
    }
}