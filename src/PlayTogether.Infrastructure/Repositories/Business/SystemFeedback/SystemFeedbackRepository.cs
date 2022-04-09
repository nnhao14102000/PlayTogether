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

        public async Task<bool> CreateFeedbackAsync(
            ClaimsPrincipal principal,
            CreateFeedbackRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (user is null) {
                return false;
            }

            if (!request.TypeOfFeedback.Equals(SystemFeedbackTypeConstants.Service)
                && !request.TypeOfFeedback.Equals(SystemFeedbackTypeConstants.SystemError)
                && !request.TypeOfFeedback.Equals(SystemFeedbackTypeConstants.Suggest)) {
                return false;
            }

            var model = _mapper.Map<Entities.SystemFeedback>(request);
            model.UserId = user.Id;
            model.IsApprove = null;
            await _context.SystemFeedbacks.AddAsync(model);

            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<PagedResult<SystemFeedbackResponse>> GetAllFeedbacksAsync(
            ClaimsPrincipal principal,
            SystemFeedbackParameters param)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            var listFeedbacks = new List<Entities.SystemFeedback>();
            if (user is null) {
                listFeedbacks = await _context.SystemFeedbacks.ToListAsync();
            }
            else {
                listFeedbacks = await _context.SystemFeedbacks.Where(x => x.UserId == user.Id).ToListAsync();
            }

            var query = listFeedbacks.AsQueryable();

            FilterByType(ref query, param.Type);
            FilterInDayRange(ref query, param.FromDate, param.ToDate);
            OrderByNewFeedback(ref query, param.IsNew);

            listFeedbacks = query.ToList();
            var response = _mapper.Map<List<SystemFeedbackResponse>>(listFeedbacks);
            return PagedResult<SystemFeedbackResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void OrderByNewFeedback(ref IQueryable<Entities.SystemFeedback> query, bool? isNew)
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

        private void FilterInDayRange(ref IQueryable<Entities.SystemFeedback> query, DateTime? fromDate, DateTime? toDate)
        {
            if (!query.Any() || fromDate is null || toDate is null) {
                return;
            }
            query = query.Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate);
        }

        private void FilterByType(ref IQueryable<Entities.SystemFeedback> query, string type)
        {
            if (!query.Any() || String.IsNullOrEmpty(type) || String.IsNullOrWhiteSpace(type)) {
                return;
            }
            query = query.Where(x => x.TypeOfFeedback.ToLower().Contains(type.ToLower()));
        }

        public async Task<SystemFeedbackDetailResponse> GetFeedbackByIdAsync(string feedbackId)
        {
            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null) {
                return null;
            }
            await _context.Entry(feedback).Reference(x => x.User).LoadAsync();
            return _mapper.Map<SystemFeedbackDetailResponse>(feedback);
        }

        public async Task<bool> ProcessFeedbackAsync(
            string feedbackId,
            ProcessFeedbackRequest request)
        {
            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null) {
                return false;
            }
            var model = _mapper.Map(request, feedback);
            _context.SystemFeedbacks.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> DeleteFeedbackAsync(ClaimsPrincipal principal, string feedbackId)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null || feedback.UserId != user.Id || feedback.IsApprove is not null) {
                return false;
            }

            _context.SystemFeedbacks.Remove(feedback);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdateFeedbackAsync(ClaimsPrincipal principal, string feedbackId, UpdateFeedbackRequest request)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return false;
            }
            var identityId = loggedInUser.Id;

            var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.IdentityId == identityId);

            var feedback = await _context.SystemFeedbacks.FindAsync(feedbackId);
            if (feedback is null || feedback.UserId != user.Id || feedback.IsApprove is not null) {
                return false;
            }
            var model = _mapper.Map(request, feedback);
            _context.SystemFeedbacks.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}