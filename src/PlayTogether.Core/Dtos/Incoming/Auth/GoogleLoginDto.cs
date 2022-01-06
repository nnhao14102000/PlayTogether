namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class GoogleLoginDto
    {
        public string ProviderName { get; set; }
        public string IdToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
