using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Parameters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Report
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IReportRepository reportRepository, ILogger<ReportService> logger)
        {
            _reportRepository = reportRepository;
            _logger = logger;
        }

        public async Task<Result<bool>> ProcessReportAsync(string reportId, ReportCheckRequest request)
        {
            try {
                if (String.IsNullOrEmpty(reportId)) {
                    throw new ArgumentNullException(nameof(reportId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _reportRepository.ProcessReportAsync(reportId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CheckReportAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<bool>> CreateReportAsync(ClaimsPrincipal principal, string orderId, ReportCreateRequest request)
        {
            try {
                if(principal is null){
                    throw new ArgumentNullException(nameof(principal));
                }
                if (String.IsNullOrEmpty(orderId)) {
                    throw new ArgumentNullException(nameof(orderId));
                }
                if (request is null) {
                    throw new ArgumentNullException(nameof(request));
                }
                return await _reportRepository.CreateReportAsync(principal, orderId, request);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call CreateReportAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<ReportGetResponse>> GetAllReportsAsync(string hirerId, ReportParamters param)
        {
            try {
                if (String.IsNullOrEmpty(hirerId)) {
                    throw new ArgumentNullException(nameof(hirerId));
                }
                return await _reportRepository.GetAllReportsAsync(hirerId, param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllReportsAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<PagedResult<ReportGetResponse>> GetAllReportsForAdminAsync(ReportAdminParameters param)
        {
            try {
                return await _reportRepository.GetAllReportsForAdminAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllReportsForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }

        public async Task<Result<ReportInDetailResponse>> GetReportInDetailByIdForAdminAsync(string reportId)
        {
            try {
                if (String.IsNullOrEmpty(reportId)) {
                    throw new ArgumentNullException(nameof(reportId));
                }
                return await _reportRepository.GetReportInDetailByIdForAdminAsync(reportId);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetReportInDetailByIdForAdminAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}