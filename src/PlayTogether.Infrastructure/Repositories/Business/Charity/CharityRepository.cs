using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Charity
{
    public class CharityRepository : BaseRepository, ICharityRepository
    {
        public CharityRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        // public async Task<PagedResult<CharityResponse>> GetAllCharitiesAsync(CharityParameters param)
        // {
        //     var charities = await _context.Charities.ToListAsync();
        //     var queryCharity = charities.AsQueryable();
            
        //     FilterActiveCharities(ref queryCharity);
        //     FilterCharitiesByName(ref queryCharity, param.Name);
            
        //     charities = queryCharity.ToList();
        //     var response = _mapper.Map<List<CharityResponse>>(charities);
        //     return PagedResult<CharityResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
        // }

        // private void FilterCharitiesByName(ref IQueryable<Entities.Charity> queryCharity, string name)
        // {
        //     if (!queryCharity.Any() || String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name)) {
        //         return;
        //     }
        //     queryCharity = queryCharity.Where(x => x.OrganizationName.ToLower().Contains(name.ToLower()));
        // }

        // private void FilterActiveCharities(ref IQueryable<Entities.Charity> queryCharity)
        // {
        //     if (!queryCharity.Any()) {
        //         return;
        //     }
        //     queryCharity = queryCharity.Where(x => x.IsActive == true);
        // }

        // public async Task<CharityResponse> GetCharityByIdAsync(string id)
        // {
        //     var charity = await _context.Charities.FindAsync(id);
        //     if (charity is null) {
        //         return null;
        //     }
        //     return _mapper.Map<CharityResponse>(charity);
        // }

        // public async Task<bool> ChangeStatusCharityByAdminAsync(string charityId, CharityStatusRequest request)
        // {
        //     var charity = await _context.Charities.FindAsync(charityId);
        //     if (charity is null) {
        //         return false;
        //     }
        //     var model = _mapper.Map(request, charity);
        //     _context.Charities.Update(model);
        //     return (await _context.SaveChangesAsync() >= 0);
        // }
    }
}
