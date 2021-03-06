using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Rank
{
    public class RankCreateRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int NO { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime CreatedDate = DateTime.UtcNow.AddHours(7);
    }
}