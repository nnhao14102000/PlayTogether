using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System;
using PlayTogether.Core.Dtos.Incoming.Generic;

namespace PlayTogether.Infrastructure.Repositories.Business.SystemFeedback
{
    public class SystemFeedbackRepository : BaseRepository, ISystemFeedbackRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public SystemFeedbackRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> CreateFeedbackAsync(
            ClaimsPrincipal principal,
            CreateFeedbackRequest request)
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

            if (!request.TypeOfFeedback.Equals(SystemFeedbackTypeConstants.Service)
                && !request.TypeOfFeedback.Equals(SystemFeedbackTypeConstants.SystemError)
                && !request.TypeOfFeedback.Equals(SystemFeedbackTypeConstants.Suggest)) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, "Vui lòng chọn loại feedback: Về dịch vụ hệ thống (Service), Về lỗi của hệ thống (System Error), Đề xuất sửa hoặc thêm tính năng, đóng góp ý kiến (Suggest)");
                return result;
            }

            var model = _mapper.Map<Core.Entities.SystemFeedback>(request);
            model.UserId = user.Id;
            model.IsApprove = null;
            await _context.SystemFeedbacks.AddAsync(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<PagedResult<SystemFeedbackResponse>> GetAllFeedbacksAsync(
            ClaimsPrincipal principal,
            SystemFeedbackParameters param)
        {
            var result = new PagedResult<SystemFeedbackResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var listFeedbacks = new List<Core.Entities.SystemFeedback>();
            if (user is null) {
                listFeedbacks = await _context.SystemFeedbacks.ToListAsync();
            }
            else {
                listFeedbacks = await _context.SystemFeedbacks.Where(x => x.UserId == user.Id).ToListAsync();
            }

            var query = listFeedbacks.AsQueryable();

            FilterByType(ref query, param.Type);
            FilterInDayRange(ref query, param.FromDate, param.ToDate);
            FilterByIsApprove(ref query, param.IsApprove, param.GetAll);
            OrderByNewFeedback(ref query, param.IsNew);

            listFeedbacks = query.ToList();
            var response = _mapper.Map<List<SystemFeedbackResponse>>(listFeedbacks);
            return PagedResult<SystemFeedbackResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterByIsApprove(ref IQueryable<Core.Entities.SystemFeedback> query, bool? isApprove, bool getAll)
        {
            if (!query.Any()) {
                return;
            }
            
            if(getAll is true){
                return;
            }else{
                query = query.Where(x => x.IsApprove == isApprove);
            }
        }

        private void OrderByNewFeedback(ref IQueryable<Core.Entities.SystemFeedback> query, bool? isNew)
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

        private void FilterInDayRange(ref IQueryable<Core.Entities.SystemFeedback> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }

            if (fromDate > toDate) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        private void FilterByType(ref IQueryable<Core.Entities.SystemFeedback> query, string type)
        {
            if (!query.Any() || String.IsNullOrEmpty(type) || String.IsNullOrWhiteSpace(type)) {
                return;
            }
            query = query.Where(x => x.TypeOfFeedback.ToLower().Contains(type.ToLower()));
        }

        public async Task<Result<SystemFeedbackDetailResponse>> GetFeedbackByIdAsync(string feedbackId)
        {
            var result = new Result<SystemFeedbackDetailResponse>();
            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound);
                return result;
            }
            await _context.Entry(feedback).Reference(x => x.User).LoadAsync();
            var response = _mapper.Map<SystemFeedbackDetailResponse>(feedback);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> ProcessFeedbackAsync(
            string feedbackId,
            ProcessFeedbackRequest request)
        {
            var result = new Result<bool>();
            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound);
                return result;
            }
            var model = _mapper.Map(request, feedback);
            _context.SystemFeedbacks.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> DeleteFeedbackAsync(ClaimsPrincipal principal, string feedbackId)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound);
                return result;
            }

            if (feedback.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            if (feedback.IsApprove is not null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Đóng góp của bạn đã được admin xử lí.");
                return result;
            }

            _context.SystemFeedbacks.Remove(feedback);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<bool>> UpdateFeedbackAsync(ClaimsPrincipal principal, string feedbackId, UpdateFeedbackRequest request)
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

            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound);
                return result;
            }

            if (feedback.UserId != user.Id) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }

            if (feedback.IsApprove is not null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, $"Đóng góp của bạn đã được admin xử lí.");
                return result;
            }
            var model = _mapper.Map(request, feedback);
            _context.SystemFeedbacks.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}