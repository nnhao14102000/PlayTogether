using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Admin
{
    public class AdminRepository : BaseRepository, IAdminRepository
    {
        public AdminRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<Result<(int, int, int, int)>> AdminStatisticAsync()
        {
            var result = new Result<(int, int, int, int)>();
            int numOfReport = 0;
            int numOfDisableUser = 0;
            int numOfSuggestFeedback = 0;
            int numOfNewUser = 0;

            numOfReport = await _context.Reports.Where(x => x.IsApprove == null).CountAsync();
            numOfDisableUser = await _context.AppUsers.Where(x => x.IsActive == false).CountAsync();
            numOfSuggestFeedback = await _context.SystemFeedbacks
                                                    .Where(x => x.IsApprove == -1).CountAsync();
            var users = await _context.AppUsers.ToListAsync();
            foreach (var item in users) {
                var date = item.CreatedDate.ToShortDateString();
                var toDay = DateTime.UtcNow.AddHours(7).ToShortDateString();
                if (date.Equals(toDay)) {
                    numOfNewUser += 1;
                }
            }
            result.Content = (numOfReport, numOfDisableUser, numOfSuggestFeedback, numOfNewUser);
            return result;
        }

        public async Task<Result<bool>> MaintainAsync()
        {
            var result = new Result<bool>();
            var users = await _context.AppUsers.ToListAsync(); // get all users
            foreach (var user in users) {
                if (user.Status is not UserStatusConstants.Maintain) { // user is not maintain
                    user.Status = UserStatusConstants.Maintain;
                    await _context.Entry(user).Collection(x => x.Orders).LoadAsync();
                    var orderProcesses = user.Orders.Where(x => x.Status == OrderStatusConstants.Processing);
                    if (orderProcesses.Count() > 0) { // get order in process
                        foreach (var order in orderProcesses) {
                            await _context.Entry(order).Reference(x => x.User).LoadAsync();
                            order.Status = OrderStatusConstants.Interrupt;
                            if (order.UserId == user.Id) {
                                var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
                                toUser.Status = UserStatusConstants.Maintain;
                            }
                            else {
                                order.User.Status = UserStatusConstants.Maintain;
                            }
                            order.Reason = "Bảo trì hệ thống";
                            order.FinalPrices = 0;
                            order.TimeFinish = DateTime.UtcNow.AddHours(7);
                        }
                    }
                    if (await _context.SaveChangesAsync() < 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                        return result;
                    }
                    var orderStarts = user.Orders.Where(x => x.Status == OrderStatusConstants.Start);
                    if (orderStarts.Count() > 0) { // get order in starting
                        foreach (var order in orderStarts) {
                            await _context.Entry(order).Reference(x => x.User).LoadAsync();
                            order.Status = OrderStatusConstants.Interrupt;
                            if (order.UserId == user.Id) {
                                var toUser = await _context.AppUsers.FindAsync(order.ToUserId);
                                toUser.Status = UserStatusConstants.Maintain;
                                await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                                    toUser.Id, "Đã hủy cuộc hẹn để bảo trì.", $"Xin chào {toUser.Name}, do bảo trì hệ thống, nên chúng tôi đã hủy cuộc hẹn này, đã có thông báo bảo trì từ trước. Xin cảm ơn.", ""
                                ));
                            }
                            else {
                                order.User.Status = UserStatusConstants.Maintain;
                                await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                                    order.User.Id, "Đã hủy cuộc hẹn để bảo trì.", $"Xin chào {order.User.Name}, do bảo trì hệ thống, nên chúng tôi đã hủy cuộc hẹn này, đã có thông báo bảo trì từ trước. Xin cảm ơn.", ""
                                ));
                                await _context.Entry(order.User).Reference(x => x.UserBalance).LoadAsync();
                                order.User.UserBalance.ActiveBalance += order.TotalPrices;
                                order.User.UserBalance.Balance += order.TotalPrices;
                                await _context.TransactionHistories.AddAsync(Helpers.TransactionHelpers.PopulateTransactionHistory(
                                    order.User.UserBalance.Id, TransactionTypeConstants.Add, order.TotalPrices, TransactionTypeConstants.MaintainRefund, order.Id
                                ));
                                order.Reason = "Bảo trì hệ thống";
                                order.FinalPrices = 0;
                                order.TimeFinish = DateTime.UtcNow.AddHours(7);
                            }
                        }
                    }
                }
                else {
                    user.Status = UserStatusConstants.Offline;
                    if (await _context.SaveChangesAsync() < 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
                        return result;
                    }
                }

            }
            result.Content = true;
            return result;
        }

        // public async Task<PagedResult<AdminResponse>> GetAllAdminsAsync(AdminParameters param)
        // {
        //     List<Entities.Admin> admins = null;

        //     admins = await _context.Admins.ToListAsync();

        //     if (admins is not null) {
        //         if (!String.IsNullOrEmpty(param.Name)) {
        //             var query = admins.AsQueryable();
        //             query = query.Where(x => (x.Lastname + x.Firstname).ToLower()
        //                                                                .Contains(param.Name.ToLower()));
        //             admins = query.ToList();
        //         }

        //         var response = _mapper.Map<List<AdminResponse>>(admins);
        //         return PagedResult<AdminResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        //     }

        //     return null;
        // }


    }
}
