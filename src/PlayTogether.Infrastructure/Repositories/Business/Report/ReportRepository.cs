using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PlayTogether.Infrastructure.Repositories.Business.Report
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ReportRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> ProcessReportAsync(string reportId, ReportCheckRequest request)
        {
            var result = new Result<bool>();
            var report = await _context.Reports.FindAsync(reportId);

            if (report is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" tố cáo này.");
                return result;
            }

            if (report.IsApprove is not null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Bạn đã xử lí tố cáo này rồi.");
                return result;
            }
            var toUser = await _context.AppUsers.FindAsync(report.ToUserId);

            await _context.Entry(toUser).Reference(x => x.BehaviorPoint).LoadAsync();

            var model = _mapper.Map(request, report);
            _context.Reports.Update(model);

            if (request.IsApprove == true) {
                if (request.IsDisableAccount is null) {
                    result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Vui lòng xác nhận có hay không khóa tài khoản người chơi.");
                    return result;
                }
                if (request.IsDisableAccount == false) {
                    if (request.Point != 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Vui lòng nhập số điểm trừ gồm điểm uy tín (Point)");
                        return result;
                    }

                    if (request.SatisfiedPoint != 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Vui lòng nhập số điểm trừ gồm điểm tích cực (SatisfiedPoint)");
                        return result;
                    }
                    await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                        report.ToUserId, "Bạn đã bị tố cáo.", $"Nội dung tố cáo: {report.ReportMessage}. Qua quá trình xét duyệt, thì tố cáo trên là đúng, phù hợp với dữ liệu thu thập được trong hệ thống. Nếu bạn tiếp tục vi phạm thì có thể sẽ bị khóa tài khoản.", ""
                    ));
                    toUser.BehaviorPoint.Point -= request.Point;
                    toUser.BehaviorPoint.SatisfiedPoint -= request.SatisfiedPoint;
                    if (toUser.BehaviorPoint.Point <= 0) {
                        toUser.BehaviorPoint.Point = 0;
                    }
                    if (toUser.BehaviorPoint.SatisfiedPoint <= 0) {
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                    }
                    await _context.BehaviorHistories.AddRangeAsync(
                        Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.ReportTrue, request.Point, BehaviorTypeConstants.Point, reportId
                        ),
                        Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.ReportTrue, request.SatisfiedPoint, BehaviorTypeConstants.SatisfiedPoint, reportId
                        )
                    );
                }
                else {
                    if (request.Point != 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Vui lòng nhập số điểm trừ gồm điểm uy tín (Point)");
                        return result;
                    }

                    if (request.SatisfiedPoint != 0) {
                        result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Vui lòng nhập số điểm trừ gồm điểm tích cực (SatisfiedPoint)");
                        return result;
                    }
                    await _context.Notifications.AddAsync(Helpers.NotificationHelpers.PopulateNotification(
                        report.ToUserId, "Bạn đã bị tố cáo.", $"Nội dung tố cáo: {report.ReportMessage}. Qua quá trình xét duyệt, thì tố cáo trên là đúng, phù hợp với dữ liệu thu thập được trong hệ thống. Sẽ có thông báo chi tiết tới bạn sau ít phút.", ""
                    ));

                    toUser.BehaviorPoint.Point -= request.Point;
                    toUser.BehaviorPoint.SatisfiedPoint -= request.SatisfiedPoint;
                    if (toUser.BehaviorPoint.Point <= 0) {
                        toUser.BehaviorPoint.Point = 0;
                    }
                    if (toUser.BehaviorPoint.SatisfiedPoint <= 0) {
                        toUser.BehaviorPoint.SatisfiedPoint = 0;
                    }
                    await _context.BehaviorHistories.AddRangeAsync(
                        Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.ReportTrue, request.Point, BehaviorTypeConstants.Point, reportId
                        ),
                        Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                            toUser.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.ReportTrue, request.SatisfiedPoint, BehaviorTypeConstants.SatisfiedPoint, reportId
                        )
                    );
                }

            }

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> CreateReportAsync(ClaimsPrincipal principal, string orderId, ReportCreateRequest request)
        {
            var result = new Result<bool>();

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

            var order = await _context.Orders.FindAsync(orderId);

            if (order is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" order này.");
                return result;
            }

            if ((order.UserId != user.Id && order.ToUserId != user.Id)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            if (order.Status is OrderStatusConstants.Start) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order chưa kết thúc.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Cancel) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đã bị hủy.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Processing) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đang được xử lí.");
                return result;
            }

            if (order.Status is OrderStatusConstants.Interrupt) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Order đã bị buộc dừng lại.");
                return result;
            }

            if (DateTime.UtcNow.AddHours(7).AddHours(-ValueConstants.HourActiveFeedbackReport) > order.TimeFinish
                // || DateTime.UtcNow.AddHours(7).AddMinutes(-ValueConstants.HourActiveFeedbackReportForTest) > order.TimeFinish
                ) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Đã hết thời gian cho phép đánh giá.");
                return result;
            }


            await _context.Entry(order).Reference(x => x.User).LoadAsync();
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);


            await _context.Entry(order).Collection(x => x.Reports).LoadAsync();
            if (order.Reports.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.UserId)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Bạn đã tố cáo rồi.");
                return result;
            }

            if (order.Reports.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.ToUserId)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Bạn đã tố cáo rồi.");
                return result;
            }

            await _context.Entry(order).Collection(x => x.Ratings).LoadAsync();
            if (order.Ratings.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.UserId)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Bạn đã đánh giá người chơi nên bạn không thể tố cáo.");
                return result;
            }

            await _context.Entry(user).Reference(x => x.BehaviorPoint).LoadAsync();
            await _context.Entry(toUser).Reference(x => x.BehaviorPoint).LoadAsync();

            if (toUser.IdentityId == identityId) {
                var model = _mapper.Map<Entities.Report>(request);
                model.OrderId = orderId;
                model.UserId = order.ToUserId;
                model.ToUserId = order.UserId;
                model.IsApprove = null;

                await _context.Reports.AddAsync(model);
                user.BehaviorPoint.SatisfiedPoint -= 1;
                await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                    user.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.Report, 1, BehaviorTypeConstants.SatisfiedPoint, model.Id
                ));
            }
            else {
                var model = _mapper.Map<Entities.Report>(request);
                model.OrderId = orderId;
                model.UserId = order.UserId;
                model.ToUserId = order.ToUserId;
                model.IsApprove = null;

                await _context.Reports.AddAsync(model);
                toUser.BehaviorPoint.SatisfiedPoint -= 1;
                await _context.BehaviorHistories.AddAsync(Helpers.BehaviorHistoryHelpers.PopulateBehaviorHistory(
                    toUser.BehaviorPoint.Id, BehaviorTypeConstants.Sub, BehaviorTypeConstants.Report, 1, BehaviorTypeConstants.SatisfiedPoint, model.Id
                ));
            }
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<ReportGetResponse>> GetAllReportsAsync(string userId, ReportParamters param)
        {
            var result = new PagedResult<ReportGetResponse>();
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.UserNotFound);
                return result;
            }

            var reports = await _context.Reports.Where(x => x.ToUserId == userId).ToListAsync();
            var query = reports.AsQueryable();

            FilterByStatus(ref query, param.IsApprove);
            FilterFromDateToDate(ref query, param.FromDate, param.ToDate);
            OrderByCreatedDate(ref query, param.IsNew);

            reports = query.ToList();
            var response = _mapper.Map<List<ReportGetResponse>>(reports);
            return PagedResult<ReportGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterFromDateToDate(ref IQueryable<Entities.Report> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        private void OrderByCreatedDate(ref IQueryable<Entities.Report> query, bool? isNew)
        {
            if (!query.Any() || isNew is null) {
                return;
            }
            if (isNew is false) {
                query = query.OrderBy(x => x.CreatedDate);
            }
            else {
                query = query.OrderByDescending(x => x.CreatedDate);
            }
        }

        public async Task<PagedResult<ReportGetResponse>> GetAllReportsForAdminAsync(ReportAdminParameters param)
        {
            var reports = await _context.Reports.ToListAsync();
            var query = reports.AsQueryable();

            FilterByStatus(ref query, param.IsApprove);
            FilterFromDateToDate(ref query, param.FromDate, param.ToDate);
            OrderByCreatedDate(ref query, param.IsNew);

            reports = query.ToList();
            var response = _mapper.Map<List<ReportGetResponse>>(reports);
            return PagedResult<ReportGetResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterByStatus(ref IQueryable<Entities.Report> query, bool? isApprove)
        {
            if (!query.Any()) {
                return;
            }

            query = query.Where(x => x.IsApprove == isApprove);

        }

        public async Task<Result<ReportInDetailResponse>> GetReportInDetailByIdForAdminAsync(string reportId)
        {
            var result = new Result<ReportInDetailResponse>();
            var report = await _context.Reports.FindAsync(reportId);
            if (report is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + $" tố cáo này.");
                return result;
            }
            await _context.Entry(report).Reference(x => x.Order).Query().Include(x => x.Ratings).LoadAsync();
            await _context.Entry(report).Reference(x => x.Order).Query().Include(x => x.Reports).LoadAsync();
            await _context.Entry(report).Reference(x => x.Order).Query().Include(x => x.GameOfOrders).LoadAsync();
            await _context.Entry(report).Reference(x => x.User).LoadAsync();
            var response = _mapper.Map<ReportInDetailResponse>(report);
            var toUser = await _context.AppUsers.FindAsync(report.ToUserId);
            response.ToUser = new Core.Dtos.Outcoming.Business.Order.OrderUserResponse {
                Id = toUser.Id,
                Avatar = toUser.Avatar,
                Name = toUser.Name,
                IsActive = toUser.IsActive,
                IsPlayer = toUser.IsPlayer,
                Status = toUser.Status
            };


            result.Content = response;
            return result;
        }
    }
}