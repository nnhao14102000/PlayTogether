using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class OrderProcessByPlayerRequest
    {
        [Required]
        public bool IsAccept { get; set; }

        [MaxLength(200)]
        public string Reason { get; set; }
    }
}