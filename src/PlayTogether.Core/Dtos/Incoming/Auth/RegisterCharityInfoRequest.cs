using System.ComponentModel.DataAnnotations;

namespace PlayTogether.Core.Dtos.Incoming.Auth
{
    public class RegisterCharityInfoRequest: RegisterRequest
    {
        public string OrganizationName { get; set; }

        public string Description { get; set; }
    }
}
