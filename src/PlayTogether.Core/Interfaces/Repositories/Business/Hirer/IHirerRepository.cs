﻿using PlayTogether.Core.Dtos.Incoming.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business.Hirer
{
    public interface IHirerRepository
    {
        Task<PagedResult<HirerGetAllResponse>> GetAllHirersAsync(HirerParameters param);
        Task<HirerProfileResponse> GetHirerProfileByIdentityIdAsync(ClaimsPrincipal principal);
        Task<HirerGetByIdForHirerResponse> GetHirerByIdAsync(string id);
        Task<bool> UpdateHirerInformationAsync(string id, HirerUpdateInfoRequest request);
    }
}