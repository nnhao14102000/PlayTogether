using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Charity
{
    public class CharityStatusRequest
    {
        [Required]
        public bool IsActive { get; set; }
    }
}