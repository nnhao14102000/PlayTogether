using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Incoming.Generic;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Charity
{
    public class CharityRepository : BaseRepository, ICharityRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public CharityRepository(IMapper mapper, AppDbContext context, UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param)
        {
            var charities = await _context.Charities.ToListAsync();
            var queryCharity = charities.AsQueryable();

            FilterActiveCharities(ref queryCharity, param.IsActive);
            FilterCharitiesByName(ref queryCharity, param.Name);

            charities = queryCharity.ToList();
            var response = _mapper.Map<List<CharityResponse>>(charities);
            return PagedResult<CharityResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterCharitiesByName(ref IQueryable<Core.Entities.Charity> queryCharity, string name)
        {
            if (!queryCharity.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            queryCharity = queryCharity.Where(x => x.OrganizationName.ToLower().Contains(name.ToLower()));
        }

        private void FilterActiveCharities(ref IQueryable<Core.Entities.Charity> queryCharity, bool? isActive)
        {
            if (!queryCharity.Any() || isActive is null) {
                return;
            }
            queryCharity = queryCharity.Where(x => x.IsActive == isActive);
        }

        public async Task<Result<CharityResponse>> GetCharityByIdAsync(string id)
        {
            var result = new Result<CharityResponse>();
            var charity = await _context.Charities.FindAsync(id);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + " không tìm thấy tổ chức từ thiện này.");
            }
            var response = _mapper.Map<CharityResponse>(charity);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request)
        {
            var result = new Result<bool>();
            var charity = await _context.Charities.FindAsync(charityId);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, ErrorMessageConstants.NotFound + " không tìm thấy tổ chức từ thiện này.");
            }
            var model = _mapper.Map(request, charity);
            _context.Charities.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }

        public async Task<Result<CharityResponse>> GetProfileAsync(ClaimsPrincipal principal)
        {
            var result = new Result<CharityResponse>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy tài khoản tổ chức của bạn.");
                return result;
            }
            var response = _mapper.Map<CharityResponse>(charity);
            result.Content = response;
            return result;
        }

        public async Task<Result<bool>> UpdateProfileAsync(ClaimsPrincipal principal, string charityId, CharityUpdateRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy tài khoản tổ chức của bạn.");
                return result;
            }

            if (charity.Id != charityId) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.NotOwnInfo);
                return result;
            }
            var model = _mapper.Map(request, charity);
            _context.Charities.Update(model);
            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;

        }

        public async Task<Result<bool>> CharityWithDrawAsync(ClaimsPrincipal principal, CharityWithDrawRequest request)
        {
            var result = new Result<bool>();
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(400, APITypeConstants.BadRequest_400, ErrorMessageConstants.Unauthenticate);
                return result;
            }
            var identityId = loggedInUser.Id;

            var charity = await _context.Charities.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            if (charity is null) {
                result.Error = Helpers.ErrorHelpers.PopulateError(404, APITypeConstants.NotFound_404, "Không tìm thấy tài khoản tổ chức của bạn.");
                return result;
            }

            var model = _mapper.Map<Core.Entities.CharityWithdraw>(request);
            model.CharityId = charity.Id;
            await _context.CharityWithdraws.AddAsync(model);

            if (await _context.SaveChangesAsync() >= 0) {
                result.Content = true;
                return result;
            }
            result.Error = Helpers.ErrorHelpers.PopulateError(0, APITypeConstants.SaveChangesFailed, ErrorMessageConstants.SaveChangesFailed);
            return result;
        }
    }
}
