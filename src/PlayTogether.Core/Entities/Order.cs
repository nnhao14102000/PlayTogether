using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayTogether.Core.Entities
{
    public class Order : BaseEntity
    {
        [MaxLength(100)]   
        [Required]     
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [MaxLength(100)]
        [Required]
        public string ToUserId { get; set; }
        //public User ToUser { get; set; }

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Report> Reports { get; set; }
        public IList<GameOfOrder> GameOfOrders { get; set; }

        public int TotalTimes { get; set; }

        [Column(TypeName = "float")]
        public float TotalPrices { get; set; }

        [Column(TypeName = "float")]
        public float FinalPrices { get; set; }

        [MaxLength(200)]
        [Required]
        public string Message { get; set; }

        [MaxLength(200)]
        public string Reason { get; set; }

        // reason order finish soon

        [MaxLength(50)]
        [Required]
        public string Status { get; set; }

        public DateTime ProcessExpired {get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeFinish {get; set; }
    }
}
