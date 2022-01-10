namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class GoogleLoginRequest
    {
        public string ProviderName { get; set; }
        public string IdToken { get; set; }
    }
}
