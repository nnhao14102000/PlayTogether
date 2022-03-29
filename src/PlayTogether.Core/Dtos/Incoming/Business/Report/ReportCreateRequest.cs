using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Report
{
    public class ReportCreateRequest
    {
        [Required]
        [MaxLength(500)]
        public string ReportMessage { get; set; }
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}