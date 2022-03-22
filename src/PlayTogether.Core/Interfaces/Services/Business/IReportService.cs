using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IReportService
    {
        Task<bool> CreateReportAsync(ClaimsPrincipal principal, string orderId, ReportCreateRequest request);
        Task<PagedResult<ReportGetResponse>> GetAllReportsForAdminAsync(ReportAdminParameters param);
        Task<PagedResult<ReportGetResponse>> GetAllReportsAsync(string userId, ReportParamters param);
        Task<ReportInDetailResponse> GetReportInDetailByIdForAdminAsync(string reportId);
        Task<bool> ProcessReportAsync(string reportId, ReportCheckRequest request);
    }
}