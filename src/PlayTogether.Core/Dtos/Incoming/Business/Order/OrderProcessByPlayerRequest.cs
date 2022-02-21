using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class OrderProcessByPlayerRequest
    {
        [Required]
        public bool IsAccept { get; set; }

        public string CharityId { get; set; }
    }
}