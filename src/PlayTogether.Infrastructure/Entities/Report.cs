﻿using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Infrastructure.Entities
{
    public class Report : BaseEntity
    {
        [MaxLength(100)]
        public string OrderId { get; set; }
        public Order Order { get; set; }

        //public User FromUser { get; set; }
        [MaxLength(100)]
        public string FromUserId { get; set; }

        //public User ToUser { get; set; }
        [MaxLength(100)]
        public string ToUserId { get; set; }

        public string ReportMessage { get; set; }
        public bool? IsApprove { get; set; }
    }
}
