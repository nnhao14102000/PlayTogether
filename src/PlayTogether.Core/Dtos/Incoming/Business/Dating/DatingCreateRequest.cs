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

        [Range(2, 8)]
        [Required]
        public int DayInWeek { get; set; }
    }
}