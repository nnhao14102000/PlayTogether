using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class FinishSoonRequest
    {
        [MaxLength(200)]
        [Required]
        public string Reason { get; set; }
    }
}