using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Momo
{
    public class WebPaymentRequest
    {
        [Required]
        [Range(1, float.MaxValue)]
        public float Amount { get; set; }
    }
}