using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class OrderCreateRequest
    {
        public DateTime CreatedDate = DateTime.Now;

        [Required]
        [Range(1, 3)]
        public int TotalTimes { get; set; }

        [MaxLength(100)]
        public string Message { get; set; }
    }
}