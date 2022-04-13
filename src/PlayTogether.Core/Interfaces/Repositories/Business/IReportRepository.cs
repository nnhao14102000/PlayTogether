using System.Security.Claims;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Parameters;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IReportRepository
    {
        Task<Result<bool>> CreateReportAsync(ClaimsPrincipal principal, string orderId, ReportCreateRequest request);
        Task<PagedResult<ReportGetResponse>> GetAllReportsForAdminAsync(ReportAdminParameters param);
        Task<PagedResult<ReportGetResponse>> GetAllReportsAsync(string userId, ReportParamters param);
        Task<Result<ReportInDetailResponse>> GetReportInDetailByIdForAdminAsync(string reportId);
        Task<Result<bool>> ProcessReportAsync(string reportId, ReportCheckRequest request);
    }
}