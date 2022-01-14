using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class GoogleLoginRequest
    {
        [Required]
        public string ProviderName { get; set; }
        [Required]
        public string IdToken { get; set; }
    }
}
