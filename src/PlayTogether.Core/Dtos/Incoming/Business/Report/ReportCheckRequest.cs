using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Report
{
    public class ReportCheckRequest
    {
        public bool? IsApprove { get; set; }
        [Range(0, 100)]
        public int Point { get; set; }
        [Range(0, 100)]
        public int SatisfiedPoint { get; set; }
        public bool? IsDisableAccount { get; set; }
    }
}