using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Donate
{
    public class DonateService : IDonateService
    {
        private readonly IDonateRepository _donateRepository;
        private readonly ILogger<DonateService> _logger;

        public DonateService(IDonateRepository donateRepository, ILogger<DonateService> logger)
        {
            _donateRepository = donateRepository;
            _logger = logger;
        }

        public async Task<bool> CreateDonateAsync(ClaimsPrincipal principal, string charityId, DonateCreateRequest request)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(charityId) || String.IsNullOrWhiteSpace(charityId)) {
                    throw new ArgumentNullException(nameof(charityId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }

                return await _donateRepository.CreateDonateAsync(principal, charityId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateDonateAsync in service class, Error Message: {ex}.");
                throw;
            }
        }


        public async Task<PagedResult<DonateResponse>> GetAllDonatesAsync(ClaimsPrincipal principal, DonateParameters param)
        {
            try {
                if (principal is null) {
                    throw new ArgumentNullException(nameof(principal));
                }

                return await _donateRepository.GetAllDonatesAsync(principal, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllDonatesAsync in service class, Error Message: {ex}.");
                throw;
            }
        }


        public async Task<DonateResponse> GetDonateByIdAsync(string donateId)
        {
            try {
                if (String.IsNullOrEmpty(donateId) || String.IsNullOrWhiteSpace(donateId)) {
                    throw new ArgumentNullException(nameof(donateId));
                }

                return await _donateRepository.GetDonateByIdAsync(donateId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetDonateByIdAsync in service class, Error Message: {ex}.");
                throw;
            }
        }


        // public async Task<(int, float, int, float)> CalculateDonateAsync(ClaimsPrincipal principal)
        // {
        //     try {
        //         if (principal is null) {
        //             throw new ArgumentNullException(nameof(principal));
        //         }

        //         return await _donateRepository.CalculateDonateAsync(principal);
        //     }
        //     catch (Exception ex) {
        //         _logger.LogError($"Error while trying to call CalculateTurnDonateInDayAsync in service class, Error Message: {ex}.");
        //         throw;
        //     }
        // }
    }
}