using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using PlayTogether.Core.Dtos.Incoming.Generic;

namespace PlayTogether.Infrastructure.Repositories.Business.Hirer
{
    public class HirerRepository : BaseRepository, IHirerRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HirerRepository(
            IMapper mapper,
            AppDbContext context,
            UserManager<IdentityUser> userManager) : base(mapper, context)
        {
            _userManager = userManager;
        }

        public async Task<PagedResult<HirerGetAllResponseForAdmin>> GetAllHirersForAdminAsync(HirerParameters param)
        {
            var hirers = await _context.Hirers.ToListAsync();
            var query = hirers.AsQueryable();

            SearchByHirerName(ref query, param.Name);
            FilterByStatus(ref query, param.Status);
            FilterActiveAccount(ref query, param.IsActive);

            hirers = query.ToList();
            var response = _mapper.Map<List<HirerGetAllResponseForAdmin>>(hirers);
            return PagedResult<HirerGetAllResponseForAdmin>.ToPagedList(response, param.PageNumber, param.PageSize);
        }

        private void FilterActiveAccount(ref IQueryable<Entities.Hirer> query, bool? isActive)
        {
            if (!query.Any() || isActive is null) {
                return;
            }
            query = query.Where(x => x.IsActive == isActive);
        }

        private void FilterByStatus(ref IQueryable<Entities.Hirer> query, string status)
        {
            if (!query.Any() || String.IsNullOrEmpty(status) || String.IsNullOrWhiteSpace(status)) {
                return;
            }
            query = query.Where(x => x.Status.ToLower().Contains(status.ToLower()));
        }

        private void SearchByHirerName(ref IQueryable<Entities.Hirer> query, string name)
        {
            if (!query.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
                return;
            }
            query = query.Where(x => (x.Firstname + " " + x.Lastname).ToLower().Contains(name.ToLower()));
        }

        public async Task<HirerGetByIdResponse> GetHirerByIdAsync(string hirerId)
        {
            var hirer = await _context.Hirers.FindAsync(hirerId);

            if (hirer is null) {
                return null;
            }
            return _mapper.Map<HirerGetByIdResponse>(hirer);
        }

        public async Task<HirerGetProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal)
        {
            var loggedInUser = await _userManager.GetUserAsync(principal);
            if (loggedInUser is null) {
                return null;
            }
            var identityId = loggedInUser.Id; //new Guid(loggedInUser.Id).ToString()

            var hirerProfile = await _context.Hirers.FirstOrDefaultAsync(x => x.IdentityId == identityId);
            return _mapper.Map<HirerGetProfileResponse>(hirerProfile);
        }

        public async Task<bool> UpdateHirerInformationAsync(string hirerId, HirerInfoUpdateRequest request)
        {
            var hirer = await _context.Hirers.FindAsync(hirerId);
            if (hirer is null) {
                return false;
            }
            _mapper.Map(request, hirer);
            _context.Hirers.Update(hirer);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<bool> UpdateHirerStatusForAdminAsync(string hirerId, HirerStatusUpdateRequest request)
        {
            var hirer = await _context.Hirers.FindAsync(hirerId);
            if (hirer is null) {
                return false;
            }
            _mapper.Map(request, hirer);
            _context.Hirers.Update(hirer);
            if (await _context.SaveChangesAsync() < 0) {
                return false;
            }
            if (request.IsActive is false) {
                hirer.Status = HirerStatusConstants.Offline;
                await _context.Entry(hirer).Collection(x => x.Orders).LoadAsync();
                var orders = hirer.Orders.Where(x => x.Status == OrderStatusConstants.Processing);
                foreach (var order in orders) {
                    await _context.Entry(order).Reference(x => x.Player).LoadAsync();
                    order.Status = OrderStatusConstants.Interrupt;
                    order.Player.Status = PlayerStatusConstants.Online;
                }
                await _context.Notifications.AddAsync(
                    new Entities.Notification{
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        UpdateDate = null,
                        ReceiverId = hirer.Id,
                        Title = $"Tài khoản của bạn đã bị khóa.😅",
                        Message = (String.IsNullOrEmpty(request.Message) || String.IsNullOrWhiteSpace(request.Message)) ? $"Bạn đã bị khóa tài khoản vì bạn đã đã hành vi không thích hợp. Hạn khóa tài khoản là đến ngày {DateTime.Now.AddDays(1)}" : $"Bạn đã bị khóa tài khoản vì: \"{request.Message}\". Hạn khóa tài khoản là đến ngày {DateTime.Now.AddDays(1)}",
                        Status = NotificationStatusConstants.NotRead
                    }
                );
                return (await _context.SaveChangesAsync() >= 0);
            }
            else {
                return (await _context.SaveChangesAsync() >= 0);
            }
        }
    }
}
