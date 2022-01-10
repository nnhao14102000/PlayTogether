namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterAdminInfoRequest: RegisterRequest
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
