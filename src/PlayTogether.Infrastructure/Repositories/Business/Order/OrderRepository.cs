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
            if (hirer is null
                || hirer.IsActive is false
                || hirer.Status is not HirerStatusConstants.Online) {
                return null;
            }

            // Check player and status of player
            var player = await _context.Players.FindAsync(playerId);
            if (player is null
                || player.Status is not PlayerStatusConstants.Online
                || player.IsActive is false) {
                return null;
            }

            // Check balance of hirer
            if ((request.TotalTimes * player.PricePerHour) > hirer.Balance) {
                return null;
            }

            var model = _mapper.Map<Entities.Order>(request);
            model.PlayerId = playerId;
            model.HirerId = hirer.Id;
            model.TotalPrices = request.TotalTimes * player.PricePerHour;
            model.Status = OrderStatusConstant.Processing;
            model.ProcessExpired = DateTime.Now.AddMinutes(1);

            _context.Orders.Add(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                player.Status = PlayerStatusConstants.Processing;
                hirer.Status = HirerStatusConstants.Processing;
                await _context.Notifications.AddAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = playerId,
                        Title = $"{hirer.Firstname} gửi lời mời đến bạn!",
                        Message = $"{request.Message}",
                        Status = NotificationStatusConstants.NotRead
                    });
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
            var query = ordersOfHirer.AsQueryable();

            FilterOrderByStatus(ref query, param.Status);
            FilterOrderRecent(ref query, param.IsNew);

            ordersOfHirer = query.ToList();

            foreach (var order in ordersOfHirer) {
                await _context.Entry(order)
               .Reference(x => x.Hirer)
               .LoadAsync();

                await _context.Entry(order)
                    .Reference(x => x.Player)
                    .LoadAsync();
            }

            var response = _mapper.Map<List<OrderGetByIdResponse>>(ordersOfHirer);
            return PagedResult<OrderGetByIdResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        public async Task<PagedResult<OrderGetByIdResponse>> GetAllOrderRequestByPlayerAsync(
            ClaimsPrincipal principal,
            PlayerOrderParameter param)
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
            var query = ordersOfPlayer.AsQueryable();

            FilterOrderByStatus(ref query, param.Status);
            FilterOrderRecent(ref query, param.IsNew);

            ordersOfPlayer = query.ToList();

            foreach (var order in ordersOfPlayer) {
                await _context.Entry(order)
               .Reference(x => x.Hirer)
               .LoadAsync();

                await _context.Entry(order)
                    .Reference(x => x.Player)
                    .LoadAsync();
            }

            var response = _mapper.Map<List<OrderGetByIdResponse>>(ordersOfPlayer);
            return PagedResult<OrderGetByIdResponse>
                .ToPagedList(
                    response,
                    param.PageNumber,
                    param.PageSize);
        }

        private void FilterOrderRecent(ref IQueryable<Entities.Order> query, bool? isNew)
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

        private void FilterOrderByStatus(ref IQueryable<Entities.Order> query, string status)
        {
            if (!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)) {
                return;
            }
            query = query.Where(x => x.Status == status);
        }

        public async Task<OrderGetByIdResponse> GetOrderByIdAsync(string id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order is null) {
                return null;
            }

            await _context.Entry(order)
               .Reference(x => x.Hirer)
               .LoadAsync();

            await _context.Entry(order)
                .Reference(x => x.Player)
                .LoadAsync();

            return _mapper.Map<OrderGetByIdResponse>(order);
        }

        public async Task<bool> CancelOrderRequestByHirerAsync(string id, ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var hirer = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (hirer is null || hirer.Status is not HirerStatusConstants.Processing) {
                return false;
            }

            var order = await _context.Orders.FindAsync(id);
            if (order is null || order.Status is not OrderStatusConstant.Processing) {
                return false;
            }

            order.Status = OrderStatusConstant.Cancel; // change status of Order

            await _context.Entry(order)
               .Reference(x => x.Player)
               .LoadAsync();

            await _context.Entry(order)
               .Reference(x => x.Hirer)
               .LoadAsync();

            order.Player.Status = PlayerStatusConstants.Online;
            order.Hirer.Status = HirerStatusConstants.Online;

            if (DateTime.Now >= order.ProcessExpired) {
                await _context.Notifications.AddAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = order.PlayerId,
                        Title = $"Bạn đã bỏ lỡ 1 đề nghị từ {order.Hirer.Firstname}",
                        Message = $"Bạn đã bỏ lỡ 1 yêu cầu từ {order.Hirer.Firstname} lúc {order.CreatedDate}",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
            }
            else {
                await _context.Notifications.AddAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = order.PlayerId,
                        Title = $"Yêu cầu đã bị Hủy bời {order.Hirer.Firstname}",
                        Message = $"Yêu cầu đã bị Hủy bời {order.Hirer.Firstname} lúc {DateTime.Now}",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
            }

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
            if (player is null || player.IsActive is false || player.Status is not PlayerStatusConstants.Processing) {
                return false;
            }

            var order = await _context.Orders.FindAsync(id);
            if (order is null || order.Status is not OrderStatusConstant.Processing) {
                return false;
            }

            await _context.Entry(order)
               .Reference(x => x.Hirer)
               .LoadAsync();
            await _context.Entry(order)
                .Reference(x => x.Player)
                .LoadAsync();

            if (request.IsAccept == false) {
                order.Status = OrderStatusConstant.Cancel;
                player.Status = PlayerStatusConstants.Online;
                order.Hirer.Status = HirerStatusConstants.Online;

                await _context.Notifications.AddRangeAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = order.HirerId,
                        Title = $"{player.Firstname} đã từ chối đề nghị!",
                        Message = $"Xin lỗi vì không thể ghép được với bạn. Mong có thể ghép với bạn vào lần sau. Chúc bạn chơi game vui vẻ!",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
            }
            else {
                if (!String.IsNullOrEmpty(request.CharityId) || !String.IsNullOrWhiteSpace(request.CharityId)) {
                    var charity = await _context.Charities.FindAsync(request.CharityId);
                    if (charity is null || charity.IsActive is false) {
                        return false;
                    }

                    await _context.Donates.AddAsync(new Entities.Donate {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        CharityId = request.CharityId,
                        PlayerId = player.Id,
                        OrderId = order.Id
                    });

                    order.Hirer.Balance = order.Hirer.Balance - order.TotalPrices;
                    charity.Balance = charity.Balance + order.TotalPrices;
                    order.Status = OrderStatusConstant.Start;

                    // Create Notification
                    await _context.Notifications.AddRangeAsync(
                        new Entities.Notification {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.Now,
                            UpdateDate = null,
                            ReceiverId = order.HirerId,
                            Title = $"{player.Lastname + " " + player.Firstname} đã quyên góp {order.TotalPrices}!",
                            Message = $"{player.Lastname + " " + player.Firstname} đã quyên góp số tiền bạn thuê tới tổ chức {charity.OrganizationName} lúc {DateTime.Now}.",
                            Status = NotificationStatusConstants.NotRead
                        },
                        new Entities.Notification {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.Now,
                            UpdateDate = null,
                            ReceiverId = order.PlayerId,
                            Title = $"{player.Firstname}, Cảm ơn bạn đã quyên góp!",
                            Message = $"Bạn đã quyên góp cho tổ chức {charity.OrganizationName} lúc {DateTime.Now} với số tiền {order.TotalPrices}. Xin gửi lời cảm ơn chân thành tới bạn và chúc bạn chơi vui vẻ!",
                            Status = NotificationStatusConstants.NotRead
                        }
                        , new Entities.Notification {
                            Id = Guid.NewGuid().ToString(),
                            CreatedDate = DateTime.Now,
                            UpdateDate = null,
                            ReceiverId = request.CharityId,
                            Title = $"{player.Firstname} đã quyên góp cho tổ chức của bạn.",
                            Message = $"{player.Lastname + " " + player.Firstname} đã quyên góp cho tổ chức của bạn lúc {DateTime.Now} với số tiền {order.TotalPrices}. Vui lòng kiểm tra tài khoản",
                            Status = NotificationStatusConstants.NotRead
                        }
                    );
                }
                else {
                    order.Hirer.Balance = order.Hirer.Balance - order.TotalPrices;
                    player.Balance = player.Balance + order.TotalPrices;
                    order.Status = OrderStatusConstant.Start;
                }

                player.Status = PlayerStatusConstants.Hiring;
                order.TimeStart = DateTime.Now;
                order.Hirer.Status = HirerStatusConstants.Hiring;
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

            if (order.Status is not OrderStatusConstant.Start) {
                return false;
            }

            await _context.Entry(order).Reference(x => x.Player).LoadAsync();
            await _context.Entry(order).Reference(x => x.Hirer).LoadAsync();
            order.Status = OrderStatusConstant.Finish;
            order.Player.Status = PlayerStatusConstants.Online;
            order.Hirer.Status = HirerStatusConstants.Online;

            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }

        public async Task<bool> FinishOrderSoonAsync(string id, ClaimsPrincipal principal, FinishSoonRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order is null) {
                return false;
            }

            if (order.Status is not OrderStatusConstant.Start) {
                return false;
            }

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            await _context.Entry(order).Reference(x => x.Player).LoadAsync();
            await _context.Entry(order).Reference(x => x.Hirer).LoadAsync();
            order.Status = OrderStatusConstant.Finish;
            order.Player.Status = PlayerStatusConstants.Online;
            order.Hirer.Status = HirerStatusConstants.Online;
            order.TimeFinish = DateTime.Now;

            if (order.Hirer.IdentityId == identityId) {
                await _context.Notifications.AddRangeAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = order.PlayerId,
                        Title = $"{order.Hirer.Firstname} đã yêu cầu kết thúc sớm",
                        Message = (String.IsNullOrEmpty(request.Message) || String.IsNullOrWhiteSpace(request.Message)) ? $"Yêu cầu đã kết thúc lúc {DateTime.Now}" : $"{order.Hirer.Firstname} đã yêu cầu kết thúc sớm với lời nhắn: {request.Message}. Yêu cầu đã kết thúc lúc {DateTime.Now}",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
            }
            else {
                await _context.Notifications.AddRangeAsync(
                    new Entities.Notification {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = order.HirerId,
                        Title = $"{order.Player.Firstname} đã yêu cầu kết thúc sớm",
                        Message = (String.IsNullOrEmpty(request.Message) || String.IsNullOrWhiteSpace(request.Message)) ? $"Yêu cầu đã kết thúc lúc {DateTime.Now}" : $"{order.Player.Firstname} đã yêu cầu kết thúc sớm với lời nhắn: {request.Message}. Yêu cầu đã kết thúc lúc {DateTime.Now}",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
            }

            if ((await _context.SaveChangesAsync() >= 0)) {
                return true;
            }
            return false;
        }
    }
}