using System;
using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Player
{
    public class PlayerServiceInfoUpdateRequest
    {
        [Required]
        public bool Status { get; set; } // true ~ ready, false ~ offline

        [Required]
        [Range(1, 3)]
        public int MaxHourHire { get; set; }

        [Required]
        [Range(0, 1000000000000)]
        public float PricePerHour { get; set; }

        public DateTime UpdateDate = DateTime.Now;
    }
}