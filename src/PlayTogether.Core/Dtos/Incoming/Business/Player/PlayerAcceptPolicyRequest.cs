using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Business.Player
{
    public class PlayerAcceptPolicyRequest
    {
        [Required]
        public bool Accept { get; set; }
    }
}