using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.AppUser
{
    public class UserIsPlayerChangeRequest
    {
        [Required]
        public bool IsPlayer { get; set; }
    }
}