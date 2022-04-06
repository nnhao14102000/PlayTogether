using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Dating
{
    public class DatingCreateRequest
    {
        [Range(0, 23)]
        [Required]
        public int FromHour { get; set; }
        
        [Range(0, 23)]
        [Required]
        public int ToHour { get; set; }

        public bool IsMON { get; set; }
        public bool IsTUE { get; set; }
        public bool IsWED { get; set; }
        public bool IsTHU { get; set; }
        public bool IsFRI { get; set; }
        public bool IsSAT { get; set; }
        public bool IsSUN { get; set; }
    }
}