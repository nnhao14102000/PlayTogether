using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class OrderCreateRequest
    {
        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);

        [Required]
        public int TotalTimes { get; set; }

        [MaxLength(100)]
        public string Message { get; set; }

        public List<GamesOfOrderCreateRequest> Games { get; set; }
    }
}