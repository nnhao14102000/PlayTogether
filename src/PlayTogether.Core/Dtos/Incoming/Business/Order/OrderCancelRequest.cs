using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class OrderCancelRequest
    {
        [MaxLength(200)]
        public string Reason { get; set; }
    }
}