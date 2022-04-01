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

        public async Task<bool> ProcessReportAsync(string reportId, ReportCheckRequest request)
        {
            var report = await _context.Reports.FindAsync(reportId);

            if (report is null) {
                return false;
            }

            var model = _mapper.Map(request, report);
            _context.Reports.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> CreateReportAsync(ClaimsPrincipal principal, string orderId, ReportCreateRequest request)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order is null
                || order.Status is OrderStatusConstants.Start
                || order.Status is OrderStatusConstants.Cancel
                || order.Status is OrderStatusConstants.Processing
                || order.Status is OrderStatusConstants.Interrupt
                // || DateTime.UtcNow.AddHours(7).AddHours(-ValueConstants.HourActiveFeedbackReport) > order.TimeFinish
                || DateTime.UtcNow.AddHours(7).AddMinutes(-ValueConstants.HourActiveFeedbackReportForTest) > order.TimeFinish
                ) {
                return false;
            }

            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;


            await _context.Entry(order).Reference(x => x.User).LoadAsync();
            var toUser = await _context.AppUsers.FindAsync(order.ToUserId);


            await _context.Entry(order).Collection(x => x.Reports).LoadAsync();
            if (order.Reports.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.UserId)) {
                return false;
            }

            if (order.Reports.Where(x => x.OrderId == order.Id).Any(x => x.ToUserId == order.ToUserId)) {
                return false;
            }

            await _context.Entry(order).Collection(x => x.Ratings).LoadAsync();
            if(order.Ratings.Where(x => x.OrderId == order.Id).Any(x => x.UserId == order.UserId)){
                return false;
            }

            if (toUser.IdentityId == identityId) {
                var model = _mapper.Map<Entities.Report>(request);
                model.OrderId = orderId;
                model.UserId = order.ToUserId;
                model.ToUserId = order.UserId;
                model.IsApprove = null;

                await _context.Reports.AddAsync(model);
            }
            else {
                var model = _mapper.Map<Entities.Report>(request);
                model.OrderId = orderId;
                model.UserId = order.UserId;
                model.ToUserId = order.ToUserId;
                model.IsApprove = null;

                await _context.Reports.AddAsync(model);
            }
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<ReportGetResponse>> GetAllReportsAsync(string userId, ReportParamters param)
        {
            var user = await _context.AppUsers.FindAsync(userId);
            if (user is null) {
                return null;
            }

            var reports = await _context.Reports.Where(x => x.ToUserId == userId).ToListAsync();
            var query = reports.AsQueryable();

            FilterByStatus(ref query, true);
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

        public async Task<ReportInDetailResponse> GetReportInDetailByIdForAdminAsync(string reportId)
        {
            var report = await _context.Reports.FindAsync(reportId);
            if (report is null) {
                return null;
            }
            await _context.Entry(report).Reference(x => x.Order).Query().Include(x => x.Ratings).LoadAsync();
            await _context.Entry(report).Reference(x => x.Order).Query().Include(x => x.Reports).LoadAsync();
            await _context.Entry(report).Reference(x => x.Order).Query().Include(x => x.GameOfOrders).LoadAsync();
            await _context.Entry(report).Reference(x => x.User).LoadAsync();
            var response = _mapper.Map<ReportInDetailResponse>(report);
            var toUser = await _context.AppUsers.FindAsync(report.ToUserId);

            response.ToUser.Id = toUser.Id;
            response.ToUser.Avatar = toUser.Avatar;
            response.ToUser.Name = toUser.Name;
            response.ToUser.IsActive = toUser.IsActive;
            response.ToUser.IsPlayer = toUser.IsPlayer;
            response.ToUser.Status = toUser.Status;
            return response;
        }
    }
}