﻿using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business.Hirer
{
    public interface IHirerService
    {
        Task<PagedResult<HirerGetAllResponseForAdmin>> GetAllHirersForAdminAsync(HirerParameters param);
        Task<HirerGetProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<HirerGetByIdResponseForHirer> GetHirerByIdForHirerAsync(string id);
        Task<bool> UpdateHirerInformationAsync(string id, HirerInfoUpdateRequest request);
    }
}
