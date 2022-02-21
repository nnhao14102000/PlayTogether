using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Order
{
    public class FinishSoonRequest
    {
        [MaxLength(100)]
        public string Message { get; set; }
    }
}